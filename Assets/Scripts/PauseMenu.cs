using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionMenu;

    private bool isPaused;
    public bool optionOpen = false;
    private PlayerController playerController;
    [SerializeField] Slider mouseBar;


   
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
        mouseBar.value+=20;
    }

    public void Back()
    {
        pauseMenu.gameObject.SetActive(true);
        optionMenu.gameObject.SetActive(false);
        optionOpen = false;
    }

}
