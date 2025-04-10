using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityElement : MonoBehaviour
{
    public GameManager gameManager; // Referencia al GameManager

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto con el que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            // Ejecutar lógica aquí (por ejemplo, aumentar el contador de fragmentos)
            gameManager.fragmentosRecolectados++;
            gameManager.CambiarEstadoFragmento("electricity");

            // Mostrar mensaje o cualquier otra acción que desees realizar
            Debug.Log("¡Has recogido un fragmento!");
            Destroy(gameObject);
        }
    }
}
