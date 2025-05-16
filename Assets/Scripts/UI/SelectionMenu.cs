using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionMenu : MonoBehaviour
{
    Vector3 targetRot;
    Vector3 currentAngle;
    int currentSelection;
    int totalPersonajes = 2;
    // Start is called before the first frame update
    void Start()
    {
        currentSelection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow) && currentSelection < totalPersonajes){
            currentAngle = transform.eulerAngles;
            targetRot = targetRot + new Vector3(0, 90, 0);
            currentSelection++;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) && currentSelection > 1){
            currentAngle = transform.eulerAngles;
            targetRot = targetRot - new Vector3(0, 90, 0);
            currentSelection--;
        }

        currentAngle = new Vector3(0, Mathf.LerpAngle(currentAngle.y, targetRot.y, 2.0f * Time.deltaTime), 0);
        transform.eulerAngles = currentAngle;
        if (currentSelection == 1 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0)))
        {
            PlayerPrefs.SetInt("PersonajeSeleccionado", 1);
            SceneManager.LoadScene("PisoSuperior");
        }
        if (currentSelection == 2 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0)))
        {
            PlayerPrefs.SetInt("PersonajeSeleccionado", 2);
            SceneManager.LoadScene("PisoSuperior");
        }
    }
}
