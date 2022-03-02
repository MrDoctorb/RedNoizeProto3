using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
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

    void ChangeButtonState(bool newState)
    {
        if(buttonPressed != newState)
        {
            foreach(IReactive output in outputs)
            {
                if(buttonPressed)
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
