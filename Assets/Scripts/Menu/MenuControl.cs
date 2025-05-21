using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public Button[] buttons; // Lista de botones del menú
    private int selectedIndex = 0;
    private float verticalInputCooldown = 0.3f; // tiempo entre movimientos
    private float lastInputTime;


    void Start()
    {
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelection(-1);
        }

        float vertical = Input.GetAxis("Vertical");

        if (Time.time - lastInputTime > verticalInputCooldown)
        {
            if (vertical < -0.5f)
            {
                MoveSelection(1); // hacia abajo
                lastInputTime = Time.time;
            }
            else if (vertical > 0.5f)
            {
                MoveSelection(-1); // hacia arriba
                lastInputTime = Time.time;
            }
        }
        
        if (Input.GetButtonDown("Submit"))
        {
            buttons[selectedIndex].onClick.Invoke(); // ejecutar acción del botón
        }
    }

    void MoveSelection(int direction)
    {
        selectedIndex += direction;

        if (selectedIndex >= buttons.Length)
            selectedIndex = 0;
        else if (selectedIndex < 0)
            selectedIndex = buttons.Length - 1;

        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }
}
