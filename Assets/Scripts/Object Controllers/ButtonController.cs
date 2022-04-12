using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper;

public class ButtonController : MonoBehaviour
{

    [SerializeField, RequireInterface(typeof(IReactive))]
    List<Component> outputs = new List<Component>();

    public bool buttonPressed = false;
    List<GameObject> collidingObjs = new List<GameObject>();

    private void OnCollisionEnter(Collision other)
    {
        //Layer 6 is moveable objects
        if ((other.gameObject.layer == 6 || other.gameObject.layer == 7) && !collidingObjs.Contains(other.gameObject))
        {
            collidingObjs.Add(other.gameObject);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.layer == 6 || other.gameObject.layer == 7)
        {
            collidingObjs.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (collidingObjs.Count >= 1)
        {
            foreach (GameObject obj in collidingObjs)
            {
                if (obj.activeSelf)
                {
                    ChangeButtonState(true);
                    return;
                }
            }
            ChangeButtonState(false);
        }
        else
        {
            ChangeButtonState(false);
        }

    }

    void ChangeButtonState(bool newState)
    {/*
        if (buttonPressed != newState)
        {*/
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
        //}
    }
}
