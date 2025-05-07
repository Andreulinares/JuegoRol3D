using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public Button[] buttons; // Lista de botones del menú
    private int selectedIndex = 0; // botón seleccionado

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
    }

    void MoveSelection(int direction)
    {
        selectedIndex += direction;

        if (selectedIndex >= buttons.Length)
            selectedIndex = 0; // Si llega al final, volver al inicio
        else if (selectedIndex < 0)
            selectedIndex = buttons.Length - 1; // Si está en el inicio, ir al final

        // Cambiar la selección en el EventSystem
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }
}
