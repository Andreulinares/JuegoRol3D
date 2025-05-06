using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuegoColumna : MonoBehaviour
{
    public ProgressManager progressManager;
    public GameManager gameManager;
    private bool jugadorCerca = false;
    public GameObject fragmentoFuegoPrefab;
    public Transform posicionColocacion;
    // Update is called once per frame
    private void Update()
    {
        if(progressManager.desbloqueadoFuego == true && progressManager.colocadoFuego == false)
        {
            if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
            {
                gameManager.ColocarFragmento("fire");
                Instantiate(fragmentoFuegoPrefab, posicionColocacion.position, Quaternion.identity);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            // Mostrar UI: "Presiona E para recoger" (opcional)
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            // Ocultar UI
        }
    }
}