using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RedNoize;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionMenu;
    [SerializeField] TextMeshProUGUI mouseValue, musicValue, sfxValue;
    private bool isPaused;
    private bool optionOpen = false;
    private PlayerController playerController;
    [SerializeField] Slider mouseSlider, musicSlider, sfxSlider;
    [SerializeField] GameObject player;
    private float cameraMax = 5;

    [SerializeField] TextMeshProUGUI dialougeText;

    private void Start()
    {
        Ref.dialougeText = dialougeText;

        playerController = FindObjectOfType<PlayerController>();
        mouseSlider.maxValue = cameraMax;
        mouseSlider.value = player.GetComponent<PlayerController>().cameraSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !optionOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
      
    }

    public void Pause()
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        pauseMenu.gameObject.SetActive(false);
        optionMenu.gameObject.SetActive(true);
        optionOpen = true;
    }

    public void Back()
    {
        pauseMenu.gameObject.SetActive(true);
        optionMenu.gameObject.SetActive(false);
        optionOpen = false;
    }

    public void MouseSlider()
    {
        playerController.cameraSensitivity = mouseSlider.value;
        mouseValue.text = mouseSlider.value.ToString("F0");
    }

    public void MusicSlider()
    {
        musicValue.text = musicSlider.value.ToString("F0");
    }

    public void SfxSlider()
    {
        sfxValue.text = sfxSlider.value.ToString("F0");
    }
}
