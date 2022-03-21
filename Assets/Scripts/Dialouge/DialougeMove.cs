using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeMove : StateMachineBehaviour
{

    [SerializeField] Vector3 endPos;
    [SerializeField] float speed;
    Animator anime;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anime = animator;
        Debug.Log(animator.GetComponent<NPCController>());
        anime.GetComponent<NPCController>().StartCoroutine(MoveTo(endPos));
    }


    IEnumerator MoveTo(Vector3 pos)
    {
        yield return new WaitForEndOfFrame();
        anime.transform.position = Vector3.MoveTowards(anime.transform.position, endPos, speed * Time.deltaTime);
        if(Vector3.Distance(anime.transform.position, endPos) > .1f)
        {
            anime.GetComponent<NPCController>().StartCoroutine(MoveTo(pos));
        }

    }
}
