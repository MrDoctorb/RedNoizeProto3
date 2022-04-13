using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDoorController : MonoBehaviour, IReactive
{
    Animator anime;
    [SerializeField] bool isOpenByDefault = true;
    void Start()
    {
        anime = GetComponent<Animator>();
        TurnOff();
    }

    public void TurnOff()
    {
        print("A");
        if (!CompareTag(PlayerController.curColor))
        {
            print("B");
            anime.SetBool("Open", isOpenByDefault);
        }
    }

    public void TurnOn()
    {
        print("C");
        if (!CompareTag(PlayerController.curColor))
        {
            print("D");
            anime.SetBool("Open", !isOpenByDefault);
        }
    }
}
