using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierraColumna : MonoBehaviour
{
    public ProgressManager progressManager;
    public GameManager gameManager;
    private bool jugadorCerca = false;
    public GameObject fragmentoTierraPrefab;
    public Transform posicionColocacion;
    // Update is called once per frame
    private void Update()
    {
        if(progressManager.desbloqueadoTierra == true && progressManager.colocadoTierra == false)
        {
            if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
            {
                gameManager.ColocarFragmento("earth");
                Instantiate(fragmentoTierraPrefab, posicionColocacion.position, Quaternion.identity);
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
