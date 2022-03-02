using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper;

public class ButtonController : MonoBehaviour
{

    [SerializeField, RequireInterface(typeof(IReactive))]
    List<Component> outputs = new List<Component>();

    public bool buttonPressed = false;


    private void OnCollisionStay(Collision other)
    {
        //Layer 6 is moveable objects
        if (other.gameObject.layer == 6)
        {
            ChangeButtonState(true);
        }
        else
        {
            ChangeButtonState(false);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.layer == 6)
        {
            ChangeButtonState(false);
        }
    }

    void ChangeButtonState(bool newState)
    {
        if (buttonPressed != newState)
        {
            buttonPressed = newState;
            foreach (IReactive output in outputs)
            {
                if (buttonPressed)
                {
                    output.TurnOn();
                }
                else
                {
                    output.TurnOff();
                }
            }
        }
    }
}
