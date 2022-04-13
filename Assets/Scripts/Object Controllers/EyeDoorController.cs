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
        if (!CompareTag(PlayerController.curColor))
        {
            anime.SetBool("Open", isOpenByDefault);
        }
    }

    public void TurnOn()
    {
        if (!CompareTag(PlayerController.curColor))
        {
            anime.SetBool("Open", !isOpenByDefault);
        }
    }
}
