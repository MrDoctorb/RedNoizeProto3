using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueChoice : StateMachineBehaviour
{
    public string[] choices;
    [SerializeField] GameObject button;
    Canvas canvas;
    GameObject[] allButtons;
    Animator anime;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anime = animator;
        allButtons = new GameObject[choices.Length];
        canvas = GameObject.FindObjectOfType<Canvas>();
        for (int i = 0; i < choices.Length; ++i)
        {
            GameObject newButton = Instantiate(button, canvas.transform);
            allButtons[i] = newButton;
            newButton.GetComponent<RectTransform>().localPosition = new Vector3(0, i * -40, 0);

            //Make a new variable for the function in order to pass the value of i instead of the reference to i
            int temp = i;
            newButton.GetComponent<Button>().onClick.AddListener(() => { ChooseOption(temp); });
            newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = choices[i];    
        }
    }
        
    void ChooseOption(int option)
    {
        anime.SetInteger("Choice", option);
        anime.SetTrigger("Finished");
        foreach(GameObject obj in allButtons)
        {
            Destroy(obj);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
