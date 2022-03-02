using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IReactive
{
    public void TurnOff()
    {
        gameObject.SetActive(true);
    }

    public void TurnOn()
    {
        gameObject.SetActive(false);
    }
}
