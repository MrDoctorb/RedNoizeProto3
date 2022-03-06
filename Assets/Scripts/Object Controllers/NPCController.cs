using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RedNoize;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Animator dialougeAnimator;
    
    public void Interact()
    {
        Ref.player.enabled = false;
        Ref.dialougeText.enabled = true;
        dialougeAnimator.StartPlayback();
    }

    void Update()
    {
        
    }
}
