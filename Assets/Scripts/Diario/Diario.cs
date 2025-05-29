using UnityEngine;

public class Diario1 : MonoBehaviour
{
    private Animator animator;
    private bool jugadorCerca = false;
    public int Pagina=1;
    public GameManager gameManager;
    public Diario diario;
    private GameObject player;

    private void Start()
    {
    }

    private void Update()
    {
        if (player == null)
        { 
            player = GameObject.FindGameObjectWithTag("Player");
        }

        animator = player.GetComponent<Animator>();

        if (jugadorCerca && Input.GetButtonDown("Interact"))
        {
            animator.SetTrigger("pickup");
            player.GetComponent<PlayerPickup>().SetDiario(this);
        }
    }

    public void EjecutarRecogida()
    {
        gameManager.SumarPorcentaje(2);
        gameManager.RecogerPagina(Pagina);
        activarPagina();
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
        diario.ActivarPaginaPorNumero(Pagina);
    }
}