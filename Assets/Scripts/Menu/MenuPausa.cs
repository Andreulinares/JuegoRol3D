using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;
    private AudioManager audioManager;
    public Slider volumeSlider;
    public Slider sfxVolumeSlider;
    public GameObject panelControles;
    private GameObject botonControles;
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
        botonControles = GameObject.Find("BotonControles");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Start"))
        {
            TogglePause();
        }
        
            if (isPaused && EventSystem.current.currentSelectedGameObject == volumeSlider.gameObject)
        {
            float input = Input.GetAxis("Horizontal"); 
            volumeSlider.value += input * Time.deltaTime * 10f; 
        }

        /*    if (EventSystem.current.currentSelectedGameObject == botonControles && Input.GetButtonDown("Submit"))
        {
            MostrarControles(); 
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

    if (isPaused)
    {
        EventSystem.current.SetSelectedGameObject(pauseMenu.transform.GetChild(0).gameObject);
    }
}

    public void IrAlMenu()
    {
        SceneManager.LoadScene("Menu"); 
    }

    public void CerrarMenu()
    {
        TogglePause();
    }

        public void MostrarControles()
    {
        panelControles.SetActive(true);  
        pauseMenu.SetActive(false);
    }

    public void CerrarControles()
    {
        panelControles.SetActive(false);  
        pauseMenu.SetActive(true);
    }
}
