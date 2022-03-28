using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RedNoize;
using TMPro;
public class GameController : MonoBehaviour
{
    //List of the temp masks, represented as colored squares.
    [SerializeField] Image[] maskIcon;
    //image of the selector
    [SerializeField] Image selector;
    //List of the image's position
    [SerializeField] Vector3[] pos = new Vector3[3];

    PlayerController pc;

    private Vector2 offset = new Vector2(-5f, 5f);
    private int currentText;

    [SerializeField] TMP_Text PopUp;

    public List<string> popUps = new List<string>();
    private void Start()
    {
        currentText = 0;
        pc = FindObjectOfType<PlayerController>();
        pos[0] = maskIcon[0].GetComponent<RectTransform>().anchoredPosition + offset;
        pos[1] = maskIcon[1].GetComponent<RectTransform>().anchoredPosition + offset;
        pos[2] = maskIcon[2].GetComponent<RectTransform>().anchoredPosition + offset;

        SetPopUps();
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
        if (Ref.player.maskActive)
        {

            selector.GetComponent<RectTransform>().anchoredPosition = maskIcon[2].GetComponent<RectTransform>().anchoredPosition +
                                                                    new Vector2(0, (Ref.player.selectedMask - 1) * 35) + offset;
        }
    }

    public IEnumerator Text(string text)
    {
        PopUp.gameObject.SetActive(true);
        PopUp.text = text;
        yield return new WaitForSeconds(4f);
        PopUp.gameObject.SetActive(false);
    }

    private void SetPopUps()
    {
        popUps.Add("Press F to wear a mask");
        popUps.Add("Use the mouse scroll wheel to change masks");

    }
}
