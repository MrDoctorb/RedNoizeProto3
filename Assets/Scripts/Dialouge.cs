using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialouge : StateMachineBehaviour
{
    public string[] dialouge;
    int currentLine = 0;
    TextMeshProUGUI text;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        text = FindObjectOfType<TextMeshProUGUI>();
        text.text = NextLine();
    }

    string NextLine()
    {
        if (currentLine < dialouge.Length)
        {
            string output = dialouge[currentLine];
            ++currentLine;
            return output;
        }
        else
        {
            return "";
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            string newLine = NextLine();
            if(newLine.Equals(""))
            {
                animator.SetTrigger("Finished");
            }
            else
            {
                text.text = newLine;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
