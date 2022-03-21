using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeMove : StateMachineBehaviour
{

    [SerializeField] Vector3 endPos;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(animator.GetComponent<NPCController>());
    }


    IEnumerator MoveTo(Vector3 pos)
    {
        yield return new WaitForEndOfFrame();

    }
}
