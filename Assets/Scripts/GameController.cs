using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RedNoize;

public class GameController : MonoBehaviour
{
    //List of the temp masks, represented as colored squares.
    [SerializeField] Image[] maskIcon;
    //image of the selector
    [SerializeField] Image selector;
    //List of the image's position
    [SerializeField] Vector3[] pos = new Vector3[3];


    PlayerController playerController;

    private Vector2 offset = new Vector2(-5f, 5f);
    private int current;
   

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        current = playerController.selectedMask;

        pos[0] = maskIcon[0].GetComponent<RectTransform>().anchoredPosition + offset;
        pos[1] = maskIcon[1].GetComponent<RectTransform>().anchoredPosition + offset;
        pos[2] = maskIcon[2].GetComponent<RectTransform>().anchoredPosition + offset;
    }
    void Update()
    {
        MaskIndicator();
    }

    private void MaskIndicator()
    {
        /*if (playerController.maskActive)
        {
            //current %= 3;
            if (Input.mouseScrollDelta.y < 0)
            {
                current++;
                current %= 3;
                selector.GetComponent<RectTransform>().anchoredPosition = pos[current];
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                current += 2;
                current %= 3;
                selector.GetComponent<RectTransform>().anchoredPosition = pos[current];
            }
        }*/
        selector.GetComponent<RectTransform>().anchoredPosition = maskIcon[2].GetComponent<RectTransform>().anchoredPosition + new Vector2(0, Ref.player.selectedMask * 35) + offset;

    }
}
