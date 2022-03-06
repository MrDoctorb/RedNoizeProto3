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
        rend.enabled = true;
        col.enabled = true;
    }

    public void TurnOn()
    {
        rend.enabled = false;
        col.enabled = false;
    }
}
