using UnityEngine;

public class Diario1 : MonoBehaviour
{
    private bool jugadorCerca = false;
    public int Pagina=1;
    public GameManager gameManager;
    public Diario diario;
    

    private void Update()
    {
        if (jugadorCerca && Input.GetButtonDown("Interact"))
        {
            gameManager.SumarPorcentaje(2);
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
        diario.pagina1= true;
        diario.activarPagina();
    }
}