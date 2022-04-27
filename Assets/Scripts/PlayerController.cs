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
    [SerializeField] LayerMask notPlayer;
    [SerializeField] int masksCollected = 0;
    bool grounded = true;
    public bool maskActive = false;
    bool canSwitchMask = true;
    public int selectedMask;
    List<GameObject> currentColorObjs = new List<GameObject>();
    List<Vector3> currentObjVelocities = new List<Vector3>();
    Rigidbody heldObject;
    [SerializeField] GameObject cursorIndicator, outlineObj;

    public static string curColor = "Red";

    GameController gc;
    void Start()
    {

        if (cursorIndicator == null)
        {
            cursorIndicator = GameObject.Find("ReticleTEMP").transform.GetChild(0).gameObject;
        }
        gc = FindObjectOfType<GameController>();
        Ref.player = this;

        //Set Variables
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();

        //Lock the Cursor
        Cursor.lockState = CursorLockMode.Locked;


        currentColorObjs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Red"));
        foreach (GameObject obj in currentColorObjs)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                currentObjVelocities.Add(rb.velocity);
            }
        }

        /*foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AnyMask"))
        {
            currentColorObjs.Add(obj);
        }*/

        cameraTint.enabled = false;
        cameraTint.material.color = new Color(1, .92f, .16f, .25f);
        ChangeMaskColor(2);

        FadeToBlack(-.25f, "");


        PickUpMask();
        PickUpMask();
        PickUpMask();
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            CameraControl();
            Movement();
            ColorControl();
            TryInteract();


            //CheckForColorCollision();
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
                StopAllCoroutines();
                maskActive = !maskActive;
                ChangeMaskColor(selectedMask);
                foreach (GameObject obj in currentColorObjs)
                {
                    // obj.SetActive(!maskActive);
                    Enable(obj, !maskActive);
                    cameraTint.enabled = maskActive;
                }
            }
        }

        if (maskActive && canSwitchMask)
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
        StartCoroutine(MaskCooldown());
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Outline"))
        {
            Destroy(obj);
        }




        cameraTint.enabled = true;
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

        curColor = colorString;

        if (heldObject != null && heldObject.CompareTag(colorString))
        {
            Drop();
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Outline"))
        {
            Destroy(obj);
        }

        foreach (GameObject obj in currentColorObjs)
        {
            //  obj.SetActive(true);
            Enable(obj, true);
            //  GameObject temp = Instantiate(obj, obj.transform.position, obj.transform.rotation);
        }
        currentObjVelocities.Clear();

        currentColorObjs = new List<GameObject>(GameObject.FindGameObjectsWithTag(colorString));

        /*foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AnyMask"))
        {
            currentColorObjs.Add(obj);
        }*/

        foreach (GameObject obj in currentColorObjs)
        {
            // obj.SetActive(false);
            Enable(obj, false);
        }

        //CheckForColorCollision();
    }

    IEnumerator MaskCooldown()
    {
        canSwitchMask = false;
        yield return new WaitForSeconds(.25f);
        canSwitchMask = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(.5f, 1, .5f));
    }

    void CheckForColorCollision()
    {
        print(Physics.BoxCast(transform.position, new Vector3(.25f, .5f, .1f), Vector3.up, Quaternion.identity, Mathf.Infinity, notPlayer));

        while (Physics.BoxCast(transform.position, new Vector3(.25f, .5f, .25f), Vector3.zero))
        {
            print("I am currently colliding with something, breaking so I don't infinite loop");
            break;
        }
    }

    void Enable(GameObject obj, bool enable)
    {
        obj.SetActive(enable);
        if (enable == false)
        {
            GameObject tempObj = Instantiate(outlineObj, obj.transform.position, obj.transform.rotation);
            tempObj.transform.localScale = obj.transform.lossyScale;
            tempObj.GetComponent<MeshFilter>().mesh = obj.GetComponent<MeshFilter>().mesh;
            tempObj.GetComponent<Renderer>().material.color = StringToColor(obj.tag);
        }

        /*obj.GetComponent<MeshRenderer>().enabled = enable;
        foreach (Collider col in obj.GetComponents<Collider>())
        {
            col.enabled = enable;
        }

        if (obj.GetComponent<Rigidbody>())
        {
            if (enable)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.None;
                rb.velocity = currentObjVelocities[0];
                currentObjVelocities.RemoveAt(0);


            }
            else
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                currentObjVelocities.Add(rb.velocity);
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }*/
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
        if (worldDirection.x != 0 || worldDirection.z != 0)
        {
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
                    //obj.SetActive(!maskActive);
                    Enable(obj, !maskActive);
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

    string NumToColorName(int colorNum)
    {
        switch (colorNum)
        {
            case 0:
                return "Red";
            case 1:
                return "Blue";
            case 2:
                return "Yellow";
            default:
                return "Invalid Color";
        }
    }

    Color NumToColor(int colorNum)
    {
        switch (colorNum)
        {
            case 0:
                return Color.red;
            case 1:
                return Color.blue;
            case 2:
                return Color.yellow;
            default:
                return Color.black;
        }
    }

    Color StringToColor(string colorName)
    {
        switch (colorName)
        {
            case "Red":
                return Color.red;
            case "Blue":
                return Color.blue;
            case "Yellow":
                return Color.yellow;
            default:
                return Color.black;
        }
    }
}
