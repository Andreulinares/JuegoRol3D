using UnityEngine;

public class Diario2 : MonoBehaviour
{
    public Animator animator;
    private bool jugadorCerca = false;
    public int Pagina=2;
    public GameManager gameManager;
    public Diario diario;
    

    private void Update()
    {
        if (jugadorCerca && Input.GetButtonDown("Interact"))
        {
            gameManager.SumarPorcentaje(5);
            gameManager.RecogerPagina(Pagina);
            activarPagina();
            animator.SetTrigger("pickup");
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
        diario.pagina2= true;
        diario.activarPagina();
    }
}