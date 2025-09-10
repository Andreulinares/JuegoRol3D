using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AguaColumna : MonoBehaviour
{
    public ProgressManager progressManager;
    public GameManager gameManager;
    private bool jugadorCerca = false;
    public GameObject fragmentoAguaPrefab;
    public Transform posicionColocacion;
    public GameObject interactionText;
    // Update is called once per frame
    private void Update()
    {
        if(progressManager.desbloqueadoAgua == true && progressManager.colocadoAgua == false)
        {
            if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
            {
                gameManager.ColocarFragmento("water");
                Instantiate(fragmentoAguaPrefab, posicionColocacion.position, Quaternion.identity);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            interactionText.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            interactionText.SetActive(false);
        }
    }
}
