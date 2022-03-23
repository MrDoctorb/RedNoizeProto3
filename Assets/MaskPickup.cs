using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPickup : MonoBehaviour
{
    [SerializeField] int color;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.PickUpMask();
            if(player.maskActive)
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