using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RedNoize;

public class NPCController : MonoBehaviour, IInteractable
{
    Animator dialougeAnimator;
    
    void Start()
    {
        dialougeAnimator = GetComponent<Animator>();
    }

    public void Interact()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        Ref.player.enabled = false;
        Ref.dialougeText.gameObject.SetActive(true);
        dialougeAnimator.enabled = true;
    }

    void Update()
    {
        if (dialougeAnimator.enabled && dialougeAnimator.GetBool("DialougeDone"))
        {
            dialougeAnimator.enabled = false;
        }
    }
}
