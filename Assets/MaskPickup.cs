using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPickup : MonoBehaviour, IReactive
{
    [SerializeField] int color;

    private void Start()
    {
        TurnOff();
    }

    public void TurnOn()
    {
        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
        {
            rend.enabled = false;
        }
    }

    public void TurnOff()
    {
        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
        {
            rend.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.PickUpMask();
            if (player.maskActive)
            {
                player.ChangeMaskColor(color);
            }
            else
            {
                player.selectedMask = color;
            }
            Destroy(gameObject);

        }
    }
}
