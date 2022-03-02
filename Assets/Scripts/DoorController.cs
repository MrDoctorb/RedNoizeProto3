using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IReactive
{
    MeshRenderer rend;
    BoxCollider col;
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        col = GetComponent<BoxCollider>();
    }

    public void TurnOff()
    {
        print("Going Off");
        rend.enabled = true;
        col.enabled = true;
    }

    public void TurnOn()
    {
        print("Going On");
        rend.enabled = false;
        col.enabled = false;
    }
}
