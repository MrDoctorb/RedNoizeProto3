using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Camera cam;
    Rigidbody rb;
    [SerializeField] float cameraSensitivity = 0, walkSpeed, jumpForce = 0;
    bool grounded = true;
    bool maskActive = false;
    int selectedMask;
    GameObject[] currentColorObjs = new GameObject[0];
    [SerializeField] Renderer cameraTint;
    void Start()
    {
        //Set Variables
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();

        //Lock the Cursor
        Cursor.lockState = CursorLockMode.Locked;


        currentColorObjs = GameObject.FindGameObjectsWithTag("Red");

        cameraTint.enabled = false;
        cameraTint.material.color = new Color(1, 0, 0, .5f);
    }

    void Update()
    {
        CameraControl();
        Movement();
        ColorControl();
    }

    /// <summary>
    /// Controls the use of the mask
    /// </summary>
    void ColorControl()
    {
        //Turns the mask on or off
        if (Input.GetKeyDown(KeyCode.Space))
        {
            maskActive = !maskActive;
            foreach (GameObject obj in currentColorObjs)
            {
                obj.SetActive(!maskActive);
                cameraTint.enabled = maskActive;
            }
        }

        //Scroll wheel switches colors
        if (Input.mouseScrollDelta.y > 0)
        {
            ChangeMaskColor(selectedMask + 1);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            ChangeMaskColor(selectedMask + 2);
        }
    }

    /// <summary>
    /// Call this to change the color of the mask and change the world accordingly
    /// </summary>
    /// <param name="color">0 = red, 1 = blue, 2 = yellow</param>
    void ChangeMaskColor(int color)
    {
        selectedMask = color;
        selectedMask %= 3;

        string colorString = "";
        switch (selectedMask)
        {
            case 0:
                colorString = "Red";
                cameraTint.material.color = new Color(1, 0, 0, .5f);
                break;
            case 1:
                colorString = "Blue";
                cameraTint.material.color = new Color(0, 0, 1, .5f);
                break;
            case 2:
                colorString = "Yellow";
                cameraTint.material.color = new Color(1, .92f, .16f, .5f);
                break;
        }

        foreach (GameObject obj in currentColorObjs)
        {
            obj.SetActive(true);
        }


        currentColorObjs = GameObject.FindGameObjectsWithTag(colorString);

        foreach (GameObject obj in currentColorObjs)
        {
            obj.SetActive(false);
        }


    }

    /// <summary>
    /// Basic character Movement
    /// </summary>
    void Movement()
    {
        //Base Movement

        Vector3 worldDirection = new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * walkSpeed,
                                rb.velocity.y, Input.GetAxis("Vertical") * Time.deltaTime * walkSpeed);
        //Change the world direction to relative movement
        rb.velocity = transform.TransformDirection(worldDirection);

        //Checking Jumping Stuff
        grounded = Physics.Linecast(transform.position - new Vector3(0, 1, 0), transform.position - new Vector3(0, 1.25f, 0));

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(0, jumpForce, 0);
        }
    }

    /// <summary>
    /// Uses the mouse to move the camera
    /// </summary>
    void CameraControl()
    {
        //Horizontal Camera Movement
        transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * cameraSensitivity;

        //Vertical Camera Movement
        Vector3 camAngle = cam.transform.eulerAngles;
        camAngle -= new Vector3(Input.GetAxis("Mouse Y"), 0, 0) * Time.deltaTime * cameraSensitivity;

        //Ensure players don't rotate vertically
        if (camAngle.x < 90 || camAngle.x > 270)
        {

            cam.transform.eulerAngles = camAngle;
        }
    }
}
