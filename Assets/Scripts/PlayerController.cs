using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Camera cam;
    Rigidbody rb;
    public float cameraSensitivity = 0;
    [SerializeField] float walkSpeed, jumpForce = 0;
    [SerializeField] float holdDistance = 1, grabDistance = 1;
    [SerializeField] Renderer cameraTint;
    [SerializeField] LayerMask layersToGrab;
    bool grounded = true;
    bool maskActive = false;
    int selectedMask;
    GameObject[] currentColorObjs = new GameObject[0];
    Rigidbody heldObject;
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
        TryPickUp();

    }
    /// <summary>
    ///Every Frame check for player input regarding picking up and setting down objects
    /// </summary>
    void TryPickUp()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * grabDistance, Color.red);

        //If Left Click and no held object, start holding it
        if (Input.GetMouseButtonDown(0) && heldObject == null)
        {
            RaycastHit ray;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out ray, grabDistance, layersToGrab);
            if (ray.collider != null)
            {
                heldObject = ray.collider.gameObject.GetComponent<Rigidbody>();
                heldObject.useGravity = false;
                StartCoroutine(Hold());
            }
        }
        else if (Input.GetMouseButtonDown(1) && heldObject != null)
        {
            heldObject.useGravity = true;
            heldObject = null;
            StopCoroutine(Hold());
        }
    }

    IEnumerator Hold()
    {
        if (heldObject != null)
        {
            if (heldObject.gameObject.activeSelf)
            {
                Vector3 currentObjPos = heldObject.transform.position;
                Vector3 endPoint = cam.transform.position + cam.transform.forward * holdDistance;

                heldObject.velocity = Vector3.zero;
                heldObject.AddForce((endPoint - currentObjPos) * 500);

                yield return new WaitForEndOfFrame();
                StartCoroutine(Hold());
            }
            else
            {
                heldObject = null;
            }
        }
    }

    /// <summary>
    /// Controls the use of the mask
    /// </summary>
    void ColorControl()
    {
        //Turns the mask on or off
        if (Input.GetKeyDown(KeyCode.F))
        {
            maskActive = !maskActive;
            foreach (GameObject obj in currentColorObjs)
            {
                obj.SetActive(!maskActive);
                cameraTint.enabled = maskActive;
            }
        }

        if (maskActive)
        {
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
        
        print(Input.GetAxis("Vertical"));
        
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