using UnityEngine;

public class PisoInferior : MonoBehaviour
{
    public GameManager.PisoActivado Inferior;
    private GameManager gameManager;

    private void Start()
    {
        // Obtén la referencia al GameManager
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (gameManager.PisoActual.ToString()!="Inferior"))
        {
            gameManager.DesactivarObjetosAnterior(gameManager.PisoActual);
            // Cambia el piso actual del GameManager
            if (gameManager != null)
            {
                
                gameManager.PisoActual = Inferior;
                Debug.Log("Activado: " + Inferior.ToString());
            }
            gameManager.ActivarObjetosActual(gameManager.PisoActual);
        }
    }
}