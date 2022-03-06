using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedMaskRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc.maskActive == true)
        {
            gameObject.SetActive(false);
        }
        else if (pc.maskActive == false)
        {
            gameObject.SetActive(true);
        }
    }
}
