using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IReactive
{
    MeshRenderer rend;
    BoxCollider col;
    [SerializeField] bool isEnabledByDefault = true;
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        col = GetComponent<BoxCollider>();

        TurnOff();
    }

    public void TurnOff()
    {
        if (!CompareTag(PlayerController.curColor))
        {
            rend.enabled = isEnabledByDefault;
            col.enabled = isEnabledByDefault;
        }
    }

    public void TurnOn()
    {
        if (!CompareTag(PlayerController.curColor))
        {
            rend.enabled = !isEnabledByDefault;
            col.enabled = !isEnabledByDefault;
        }
    }
}
