using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private PauseMenu pauseMenu;
    [SerializeField] Slider audioBar, sounEffectdBar;
    [SerializeField] TextMeshProUGUI audioText, soundEffectText;
    private float cameraMax = 5;

    private void Start()
    {
    
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void AudioSlider()
    {
        audioText.text = audioBar.value.ToString("F0");
    }

    public void SoundEffectSlider()
    {
        soundEffectText.text = sounEffectdBar.value.ToString("F0");
    }
}
