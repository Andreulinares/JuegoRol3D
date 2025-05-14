using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diario : MonoBehaviour
{
    public GameObject panelDiario;
    private bool diarioAbierto = false;
    // Start is called before the first frame update
    void Start()
    {
        panelDiario.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            ToggleDiario();
        }
    }

    private void ToggleDiario()
    {
        diarioAbierto = !diarioAbierto;
        panelDiario.SetActive(diarioAbierto);
        
        Time.timeScale = diarioAbierto ? 0 : 1;
    }
}
