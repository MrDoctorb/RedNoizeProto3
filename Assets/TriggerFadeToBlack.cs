using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedNoize;

public class TriggerFadeToBlack : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Ref.player.gameObject)
        {
            Ref.player.FadeToBlack(.2f, sceneToLoad);
        }
    }
}
