using UnityEngine;

public class Diario1 : MonoBehaviour
{
    private bool jugadorCerca = false;
    public int Pagina=1;
    public GameManager gameManager;
    

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            gameManager.SumarPorcentaje(2);
            gameManager.RecogerPagina(Pagina);
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