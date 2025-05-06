using UnityEngine;
using System.Collections.Generic;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;

    // Progreso de elementos
    public int porcentajeDeJuego = 0;
    public int fragmentosRecolectados = 0;
    public int fragmentosColocados = 0;
    public bool desbloqueadoAgua = false;
    public bool desbloqueadoFuego = false;
    public bool desbloqueadoTierra = false;
    public bool desbloqueadoElectricidad = false;
    public bool colocadoAgua = false;
    public bool colocadoFuego = false;
    public bool colocadoTierra = false;
    public bool colocadoElectricidad = false;

    // Páginas recogidas
    public HashSet<int> paginasRecogidas = new HashSet<int>();

    // Último punto de guardado
    public Transform ultimoPuntoGuardado;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DesbloquearElemento(string elemento)
    {
        switch (elemento.ToLower())
        {
            case "agua": desbloqueadoAgua = true; break;
            case "fuego": desbloqueadoFuego = true; break;
            case "tierra": desbloqueadoTierra = true; break;
            case "electricidad": desbloqueadoElectricidad = true; break;
            default:
                Debug.LogWarning("Elemento no reconocido: " + elemento);
                return;
        }

        Debug.Log($"Elemento desbloqueado: {elemento}");
    }
    public void ColocarElemento(string elemento)
    {
        switch (elemento.ToLower())
        {
            case "agua": colocadoAgua = true; break;
            case "fuego": colocadoFuego = true; break;
            case "tierra": colocadoTierra = true; break;
            case "electricidad": colocadoElectricidad = true; break;
            default:
                Debug.LogWarning("Elemento no reconocido: " + elemento);
                return;
        }

        Debug.Log($"Elemento colocado: {elemento}");
    }

    public void RecogerPagina(int numeroPagina)
    {
        if (paginasRecogidas.Add(numeroPagina))
        {
            Debug.Log($"Página {numeroPagina} recogida.");
        }
    }

    public bool TienePagina(int numeroPagina)
    {
        return paginasRecogidas.Contains(numeroPagina);
    }

    public void GuardarPunto(Transform punto)
    {
        ultimoPuntoGuardado = punto;
        Debug.Log("Punto de guardado actualizado.");
    }

    public void RestaurarEnPuntoGuardado(GameObject jugador)
    {
        if (ultimoPuntoGuardado != null && jugador != null)
        {
            jugador.transform.position = ultimoPuntoGuardado.position;
            Debug.Log("Jugador restaurado al último punto guardado.");
        }
    }
}
