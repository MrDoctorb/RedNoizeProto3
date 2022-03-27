using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeObjectTrigger : StateMachineBehaviour
{
    [SerializeField] string[] objectKeys;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPCController npc = animator.transform.GetComponent<NPCController>();
        foreach(string key in objectKeys)
        {
            npc.objs[key].GetComponent<IReactive>().TurnOn();
        }
    }

}
