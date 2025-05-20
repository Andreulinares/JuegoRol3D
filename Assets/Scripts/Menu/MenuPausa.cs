using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;
    private AudioManager audioManager;
    public Slider volumeSlider;
    public Slider sfxVolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            volumeSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
            volumeSlider.value = audioManager.musicSource.volume;
            
            sfxVolumeSlider.onValueChanged.AddListener(audioManager.SetSFXVolume);
            sfxVolumeSlider.value = audioManager.sfxSource.volume;  
        }
        else
        {
            Debug.LogWarning("AudioManager no encontrado en la escena.");
        }

        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        /*if (Input.GetButtonDown("StartButton"))
        {
            TogglePause();
        }*/
    }
    
    void TogglePause()
{
    isPaused = !isPaused;

    // Activar o desactivar el menú
    pauseMenu.SetActive(isPaused);

    // Pausar o reanudar el juego
    Time.timeScale = isPaused ? 0 : 1;

    Cursor.visible = isPaused;
    Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
}
}
