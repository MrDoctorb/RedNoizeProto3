using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedNoize;

public class EndPodium : MonoBehaviour
{
    [SerializeField] GameObject endDoor;
    private void OnTriggerEnter(Collider other)
    {
        int masksRemaining = Ref.player.PickUpMask(false);
        transform.GetChild(masksRemaining).gameObject.SetActive(true);

        if(masksRemaining == 0)
        {
            endDoor.SetActive(false);
        }
        Destroy(this);
    }
}
