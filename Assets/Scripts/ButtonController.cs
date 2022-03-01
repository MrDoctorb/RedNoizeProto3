using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool buttonPressed = false;
    private void OnCollisionStay(Collision other)
    {
        //Layer 6 is moveable objects
        if (other.gameObject.layer == 6)
        {
            buttonPressed = true;
        }
        else
        {
            buttonPressed = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == 6)
        {
            buttonPressed = false;

        }
    }
}
