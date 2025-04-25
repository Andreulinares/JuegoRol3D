using UnityEngine;

public class TriggerCambioPiso : MonoBehaviour
{
    public string ActivarSuperior = "ActivarPisoSuperior"; // Tag del trigger con el que debe colisionar
    public string ActivarInferior = "ActivarPisoInferior";
    public string ActivarAfueras = "ActivarLasAfueras";
    public GameManager.PisoActivado Superior; // Piso a activar
    public GameManager.PisoActivado Inferior;
    public GameManager.PisoActivado Afueras;
    public GameObject[] objetosSuperior; // Objetos a desactivar en el piso superior
    public GameObject[] objetosInferior; // Objetos a desactivar en el piso inferior
    public GameObject[] objetosAfueras; // Objetos a desactivar en las afueras

    private GameManager gameManager;

    private void Start()
    {
        // Obtén la referencia al GameManager
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ActivarSuperior) && (gameManager.PisoActual.ToString()!="Superior"))
        {
            DesactivarObjetosAnterior(gameManager.PisoActual);
            // Cambia el piso actual del GameManager
            if (gameManager != null)
            {
                
                gameManager.PisoActual = Superior;
                Debug.Log("Activado: " + Superior.ToString());
            }
        }
        else if (other.CompareTag(ActivarInferior) && (gameManager.PisoActual.ToString()!="Inferior"))
        {
            DesactivarObjetosAnterior(gameManager.PisoActual);
            // Cambia el piso actual del GameManager
            if (gameManager != null)
            {
                
                gameManager.PisoActual = Inferior;
                Debug.Log("Activado: " + Inferior.ToString());
            }
        }
        else if (other.CompareTag(ActivarAfueras) && (gameManager.PisoActual.ToString()!="Afueras"))
        {
            DesactivarObjetosAnterior(gameManager.PisoActual);
            // Cambia el piso actual del GameManager
            if (gameManager != null)
            {
                
                gameManager.PisoActual = Afueras;
                Debug.Log("Activado: " + Afueras.ToString());
            }
        }
    }

    private void DesactivarObjetosAnterior(GameManager.PisoActivado piso)
    {
        // Desactiva los objetos correspondientes al piso actual
        switch (piso)
        {
            case GameManager.PisoActivado.Superior:
                DesactivarObjetos(objetosSuperior);
                break;
            case GameManager.PisoActivado.Inferior:
                DesactivarObjetos(objetosInferior);
                break;
            case GameManager.PisoActivado.Afueras:
                DesactivarObjetos(objetosAfueras);
                break;
        }
    }

    private void DesactivarObjetos(GameObject[] objetos)
    {
        foreach (GameObject obj in objetos)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
    //Hacer funcion de desactivar superior y afueras para el start
}
