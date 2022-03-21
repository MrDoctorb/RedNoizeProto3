using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RedNoize;

public class DialougeEnd : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ref.dialougeText.gameObject.SetActive(false);
        Ref.player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator.SetBool("DialougeDone", true);

        animator.enabled = false;
    }
}
