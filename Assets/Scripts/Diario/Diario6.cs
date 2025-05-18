using UnityEngine;

public class Diario6 : MonoBehaviour
{
    private bool jugadorCerca = false;
    public int Pagina=6;
    public GameManager gameManager;
    public Diario diario;
    

    private void Update()
    {
        if (jugadorCerca && Input.GetButtonDown("Interact"))
        {
            gameManager.SumarPorcentaje(5);
            gameManager.RecogerPagina(Pagina);
            activarPagina();
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
    public void activarPagina()
    {
        diario.pagina6= true;
        diario.activarPagina();
    }
}