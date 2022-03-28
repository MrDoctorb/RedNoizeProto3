using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IReactive
{
    MeshRenderer rend;
    BoxCollider col;
    bool isEnabledByDefault;
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        col = GetComponent<BoxCollider>();
        isEnabledByDefault = gameObject.activeSelf;
    }

    public void TurnOff()
    {
        rend.enabled = isEnabledByDefault;
        col.enabled = isEnabledByDefault;
    }

    public void TurnOn()
    {
        rend.enabled = !isEnabledByDefault;
        col.enabled = !isEnabledByDefault;
    }
}
