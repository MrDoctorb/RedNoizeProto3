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

    [SerializeField] TMP_Text popText;

    public List<string> popList = new List<string>();
    public bool isTriggered;

    private void Start()
    {
        currentText = 0;
        pc = FindObjectOfType<PlayerController>();

        SetImagePOs();
      SetPopUps();
    }
    void Update()
    {
        MaskIndicator();
    }
    public void StartPopUp()
    {
       
          StartCoroutine(Text(popList[currentText]));
          currentText++;
         print(currentText);
    
    }

    private void MaskIndicator()
    {
        if (Ref.player.maskActive)
        {

            selector.GetComponent<RectTransform>().anchoredPosition = maskIcon[2].GetComponent<RectTransform>().anchoredPosition +
                                                                    new Vector2(0, (Ref.player.selectedMask - 1) * 35) + offset;
        }
    }

    public IEnumerator Text(string text)
    {
        popText.gameObject.SetActive(true);
        popText.text = text;
        yield return new WaitForSeconds(4f);
        popText.gameObject.SetActive(false);
    }
  
    private void SetPopUps()
    {
        popList.Add("Left click to interact");
        popList.Add("Press F to put on the mask");
        popList.Add("Use the mouse scroll wheel to switch between masks");
        popList.Add("Left click to pick up and drop boxes");
        popList.Add("Right click to throw boxes");
    }
    private void SetImagePOs()
    {
        pos[0] = maskIcon[0].GetComponent<RectTransform>().anchoredPosition + offset;
        pos[1] = maskIcon[1].GetComponent<RectTransform>().anchoredPosition + offset;
        pos[2] = maskIcon[2].GetComponent<RectTransform>().anchoredPosition + offset;
    }
}
