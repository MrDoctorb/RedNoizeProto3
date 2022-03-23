using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RedNoize;

public class NPCController : MonoBehaviour, IInteractable
{
    Animator dialougeAnimator;

    [SerializeField] VisibleDictionaryWorkaround[] reactiveObjects;
    public Dictionary<string, GameObject> objs;
    
    void Start()
    {
        dialougeAnimator = GetComponent<Animator>();
        objs = new Dictionary<string, GameObject>();
        foreach(VisibleDictionaryWorkaround workaround in reactiveObjects)
        {
            objs.Add(workaround.key, workaround.value);
        }
    }

    public void Interact()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        Ref.player.enabled = false;
        Ref.dialougeText.gameObject.SetActive(true);
        dialougeAnimator.enabled = true;
        if(dialougeAnimator.GetBool("DialougeDone"))
        {
            dialougeAnimator.Rebind();
            dialougeAnimator.Update(0f);
        }
    }
}

[System.Serializable]
public struct VisibleDictionaryWorkaround
{
    public string key;
    public GameObject value;
}
