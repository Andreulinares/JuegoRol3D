using UnityEngine;

public class LasAfueras : MonoBehaviour
{
    public GameManager.PisoActivado Afueras;
    private GameManager gameManager;

    private void Start()
    {
        // Obtén la referencia al GameManager
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (gameManager.PisoActual.ToString()!="Afueras"))
        {
            gameManager.DesactivarObjetosAnterior(gameManager.PisoActual);
            // Cambia el piso actual del GameManager
            if (gameManager != null)
            {
                
                gameManager.PisoActual = Afueras;
                Debug.Log("Activado: " + Afueras.ToString());
            }
            gameManager.ActivarObjetosActual(gameManager.PisoActual);
        }
    }
}