using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionMenu : MonoBehaviour
{
    Vector3 targetRot;
    Vector3 currentAngle;
    public int currentSelection;
    public int totalPersonajes = 2;

    float stickCooldown = 0.5f; // Tiempo entre cada cambio con joystick
    float stickTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentSelection = 1;
        targetRot = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        stickTimer -= Time.deltaTime;

        // Teclado
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentSelection < totalPersonajes)
        {
            RotateRight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentSelection > 1)
        {
            RotateLeft();
        }

        // Mando
        float hInput = Input.GetAxis("Horizontal");
        if (stickTimer <= 0f)
        {
            if (hInput > 0.5f && currentSelection < totalPersonajes)
            {
                RotateRight();
                stickTimer = stickCooldown;
            }
            else if (hInput < -0.5f && currentSelection > 1)
            {
                RotateLeft();
                stickTimer = stickCooldown;
            }
        }

        currentAngle = new Vector3(0, Mathf.LerpAngle(currentAngle.y, targetRot.y, 5.0f * Time.deltaTime), 0);
        transform.eulerAngles = currentAngle;
        if (currentSelection == 1 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0)))
        {
            Debug.Log("Seleccionado personaje 1, cargando escena...");
            PlayerPrefs.SetInt("PersonajeSeleccionado", currentSelection);
            Debug.Log("Personaje guardado en PlayerPrefs: " + PlayerPrefs.GetInt("PersonajeSeleccionado"));
            SceneManager.LoadScene("Prueba Juntar");
        }
        if (currentSelection == 2 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0)))
        {
            Debug.Log("Seleccionado personaje 2, cargando escena...");
            PlayerPrefs.SetInt("PersonajeSeleccionado", currentSelection);
            Debug.Log("Personaje guardado en PlayerPrefs: " + PlayerPrefs.GetInt("PersonajeSeleccionado"));
            SceneManager.LoadScene("Prueba Juntar");
        }
    }

    void RotateRight()
    {
        currentAngle = transform.eulerAngles;
        targetRot += new Vector3(0, 90, 0);
        currentSelection++;
    }

    void RotateLeft()
    {
        currentAngle = transform.eulerAngles;
        targetRot -= new Vector3(0, 90, 0);
        currentSelection--;
    }
}
