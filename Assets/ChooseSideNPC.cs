using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedNoize;
public class ChooseSideNPC : MonoBehaviour
{
    [SerializeField] NPCController npc;
    Animator anime;
    // Start is called before the first frame update
    void Start()
    {
        anime = npc.GetComponent<Animator>();
        anime.SetInteger("Choice", 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Ref.player.gameObject)
        {
            anime.SetInteger("Choice", 1);
        }
    }
}
