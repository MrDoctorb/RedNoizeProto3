using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RedNoize;

public class PlayerController : MonoBehaviour
{
    Camera cam;
    Rigidbody rb;
    public float cameraSensitivity = 0;
    [SerializeField] float walkSpeed, jumpForce = 0;
    [SerializeField] float holdDistance = 1, grabDistance = 1, throwPower;
    [SerializeField] Renderer cameraTint;
    [SerializeField] LayerMask layersToGrab;
    [SerializeField] int masksCollected = 0;
    bool grounded = true;
    public bool maskActive = false;
    public int selectedMask;
    List<GameObject> currentColorObjs = new List<GameObject>();
    Rigidbody heldObject;
    [SerializeField] GameObject cursorIndicator;

    GameController gc;
    void Start()
    {
        gc = FindObjectOfType<GameController>();
        Ref.player = this;

        //Set Variables
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();

        //Lock the Cursor
        Cursor.lockState = CursorLockMode.Locked;


        currentColorObjs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Red"));

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AnyMask"))
        {
            currentColorObjs.Add(obj);
        }

        cameraTint.enabled = false;
        cameraTint.material.color = new Color(1, .92f, .16f, .25f);
        ChangeMaskColor(2);

        FadeToBlack(-.25f, "");
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            CameraControl();
            Movement();
            ColorControl();
            TryInteract();
        }
    }
    /// <summary>
    ///Every Frame check for player input regarding picking up and setting down objects
    /// </summary>
    void TryInteract()
    {
        RaycastHit ray;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out ray, grabDistance);
        if (ray.collider != null)
        {
            if (ray.collider.gameObject.layer == 6 || ray.collider.GetComponent<NPCController>())
            {
                cursorIndicator.SetActive(true);
            }
            else
            {
                cursorIndicator.SetActive(false);
            }
        }
        else
        {
            cursorIndicator.SetActive(false);
        }
        Debug.DrawRay(cam.transform.position, cam.transform.forward * grabDistance, Color.red);

        //If Left Click and no held object,try to interact
        if (Input.GetMouseButtonDown(0) && heldObject == null)
        {
            Physics.Raycast(cam.transform.position, cam.transform.forward, out ray, grabDistance, layersToGrab);
            if (ray.collider != null)
            {
                heldObject = ray.collider.gameObject.GetComponent<Rigidbody>();
                heldObject.useGravity = false;
                heldObject.transform.parent = null;
                StartCoroutine(Hold());
                return;
            }
            Physics.Raycast(cam.transform.position, cam.transform.forward, out ray, grabDistance);

            //This will later be replaced with IInteractable functionality
            if (ray.collider != null && ray.collider.GetComponent<NPCController>())
            {
                ray.collider.GetComponent<NPCController>().Interact();
            }
        }
        else if (heldObject != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Drop();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Throw();
            }
        }
    }

    void Drop()
    {
        heldObject.useGravity = true;
        heldObject = null;
        StopCoroutine(Hold());
    }

    void Throw()
    {
        heldObject.velocity = cam.transform.forward * throwPower;
        Drop();
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
                Drop();
            }
        }
    }

    /// <summary>
    /// Controls the use of the mask
    /// </summary>
    void ColorControl()
    {
        //Only can turn mask on with this if
        if (maskActive == false)
        {
            //Turns the mask on or off
            if (Input.GetKeyDown(KeyCode.F) && masksCollected > 0)
            {
                maskActive = !maskActive;
                ChangeMaskColor(selectedMask);
                foreach (GameObject obj in currentColorObjs)
                {
                    obj.SetActive(!maskActive);
                    cameraTint.enabled = maskActive;
                }
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
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeMaskColor(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeMaskColor(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeMaskColor(0);
            }
        }
    }

    /// <summary>
    /// Call this to change the color of the mask and change the world accordingly
    /// </summary>
    /// <param name="color">0 = red, 1 = blue, 2 = yellow</param>
    public void ChangeMaskColor(int color)
    {
        selectedMask = color;
        selectedMask %= 3;
        selectedMask = Mathf.Clamp(selectedMask, 3 - masksCollected, 2);
        string colorString = "";
        switch (selectedMask)
        {
            case 0:
                colorString = "Red";
                cameraTint.material.color = new Color(1, 0, 0, .25f);
                break;
            case 1:
                colorString = "Blue";
                cameraTint.material.color = new Color(0, 0, 1, .25f);
                break;
            case 2:
                colorString = "Yellow";
                cameraTint.material.color = new Color(1, .92f, .16f, .25f);
                break;
            default:
                return;
        }

        foreach (GameObject obj in currentColorObjs)
        {
            obj.SetActive(true);
        }


        currentColorObjs = new List<GameObject>(GameObject.FindGameObjectsWithTag(colorString));

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AnyMask"))
        {
            currentColorObjs.Add(obj);
        }

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
        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= 2;
        }



        //Base Movement

        Vector3 worldDirection = new Vector3(Input.GetAxis("Horizontal") * currentSpeed,
                                rb.velocity.y, Input.GetAxis("Vertical") * currentSpeed);


        //Change the world direction to relative movement
        rb.velocity = transform.TransformDirection(worldDirection);


        Debug.DrawLine(cam.transform.position - new Vector3(0, 1, 0), transform.position - new Vector3(0, 1.25f, 0), Color.green);
        //Checking Jumping Stuff
        grounded = Physics.Linecast(cam.transform.position - new Vector3(0, 1, 0), transform.position - new Vector3(0, 1.25f, 0));

        // print(grounded);
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
        transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X"), 0) * cameraSensitivity;

        //Vertical Camera Movement
        Vector3 camAngle = cam.transform.eulerAngles;
        camAngle -= new Vector3(Input.GetAxis("Mouse Y"), 0, 0) * cameraSensitivity;

        //Ensure players don't rotate vertically
        if (camAngle.x < 90 || camAngle.x > 270)
        {

            cam.transform.eulerAngles = camAngle;
        }
    }

    public int PickUpMask(bool pickUp = true)
    {
        if (pickUp)
        {
            ++masksCollected;
            if (masksCollected > 3)
            {
                masksCollected = 3;
                Debug.LogWarning("You have picked up more than 3 masks! 0-0");
            }
        }
        else
        {
            --masksCollected;
            if (masksCollected > 0)
            {
                ChangeMaskColor(masksCollected - 1);
            }
            else
            {
                maskActive = false;
                cameraTint.enabled = maskActive;
                foreach (GameObject obj in currentColorObjs)
                {
                    obj.SetActive(!maskActive);
                }
            }
        }
        return masksCollected;
    }

    public void FadeToBlack(float fadeSpeed, string sceneToLoad)
    {
        cameraTint.enabled = true;

        if (fadeSpeed < 0)
        {
            cameraTint.material.color = new Color(0, 0, 0, 1);
        }
        else
        {
            cameraTint.material.color = new Color(0, 0, 0, 0);
        }
        StartCoroutine(Fade(fadeSpeed, sceneToLoad));
    }

    IEnumerator Fade(float fadeSpeed, string sceneToLoad)
    {
        cameraTint.material.color += new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
        yield return new WaitForEndOfFrame();
        if (cameraTint.material.color.a <= 0)
        {
            cameraTint.enabled = false;
        }
        else if (cameraTint.material.color.a >= 1)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            StartCoroutine(Fade(fadeSpeed, sceneToLoad));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TextPopup"))
        {
            gc.StartPopUp();
            other.gameObject.SetActive(false);
        }
    }
}
