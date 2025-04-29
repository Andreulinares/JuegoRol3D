using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Fragmentos del jefe
    [Header("Fragmentos")]
    public int fragmentosRecolectados = 0;
    public int fragmentosTotales = 4;
    public bool coleccionadoAgua = false;
    public bool columnaAgua = false;
    public bool coleccionadoFuego = false;
    public bool columnaFuego = false;
    public bool coleccionadoTierra = false;
    public bool columnaTierra = false;
    public bool coleccionadoElectricidad = false;
    public bool columnaElectricidad = false;
    public bool WaterEffect = true;
    public bool FireEffect = true;
    public bool EarthEffect = true;
    public bool ElectricityEffect = true;

    // Boss y elementos
    public enum ElementoActivo { Ninguno, Fuego, Agua, Electricidad, Tierra }
    public ElementoActivo elementoBoss = ElementoActivo.Ninguno;

    // Estados del juego
    public enum EstadoJuego { Jugando, Pausado, Ganado, Perdido }
    public EstadoJuego estadoActual = EstadoJuego.Jugando;
    public enum PisoActivado { Superior, Inferior, Afueras }
    public PisoActivado PisoActual = PisoActivado.Inferior;
    public GameObject[] objetosSuperior; // Objetos a desactivar en el piso superior
    public GameObject[] objetosInferior; // Objetos a desactivar en el piso inferior
    public GameObject[] objetosAfueras; // Objetos a desactivar en las afueras

    // Música
    public AudioSource musicaFondo;
    public AudioClip musicaNormal;
    public AudioClip musicaBoss;

    // Jugador
    public PlayerController jugador;
    public bool jugadorMelee=true;

    // Eventos globales
    public UnityEvent onFragmentoRecolectado;
    public UnityEvent onTodosFragmentosRecolectados;
    public UnityEvent onBossDerrotado;

    private void Awake()
    {
        // Singleton pattern
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

    private void Start()
    {
        ComienzoPiso();
        if (musicaFondo != null && musicaNormal != null)
        {
            CambiarMusica(musicaNormal);
        }
        
    }

    // Control de fragmentos
    public void RecolectarFragmento()
    {
        fragmentosRecolectados++;
        onFragmentoRecolectado?.Invoke();

        if (fragmentosRecolectados >= fragmentosTotales)
        {
            onTodosFragmentosRecolectados?.Invoke();
        }
    }

    public void ActivarBossFinal()
    {
        CambiarMusica(musicaBoss);
        Debug.Log("¡Boss activado!");
        // Aquí puedes activar el boss físicamente o cargar su escena
    }

    // Cambio de escena
    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // Control de estado del juego
    public void CambiarEstado(EstadoJuego nuevoEstado)
    {
        estadoActual = nuevoEstado;
        Time.timeScale = (estadoActual == EstadoJuego.Pausado) ? 0 : 1;

        // Aquí podrías invocar un evento o notificar a la UI
    }

    // Control de elementos del boss
    public void CambiarElementoBoss(ElementoActivo nuevoElemento)
    {
        elementoBoss = nuevoElemento;
        Debug.Log("Elemento del Boss cambiado a: " + elementoBoss);
        // Puedes activar efectos visuales, resistencias, animaciones...
    }

    // Vida del jugador
    public void RestarVidaJugador(int cantidad)
    {
        if (jugador != null)
        {
            jugador.TakeDamage(cantidad);
        }
    }

    // Música
    public void CambiarMusica(AudioClip nuevaMusica)
    {
        if (musicaFondo == null || nuevaMusica == null) return;

        musicaFondo.clip = nuevaMusica;
        musicaFondo.Play();
    }

    // Boss derrotado
    public void BossDerrotado()
    {
        CambiarEstado(EstadoJuego.Ganado);
        onBossDerrotado?.Invoke();
        Debug.Log("¡Boss derrotado!");
        // Aquí podrías mostrar créditos, una escena final, etc.
    }
    public void CambiarEstadoFragmento(string fragmento)
    {
        switch (fragmento)
        {
            case "water":
                WaterEffect = false;
                coleccionadoAgua=true;
                break;
            case "fire":
                FireEffect = false;
                coleccionadoFuego=true;
                break;
            case "earth":
                EarthEffect = false;
                coleccionadoTierra=true;
                break;
            case "electricity":
                ElectricityEffect = false;
                coleccionadoElectricidad=true;
                break;
            default:
                Debug.LogError("Fragmento no reconocido");
                break;
        }
    }
    public void DesactivarObjetos(GameObject[] objetos)
    {
        foreach (GameObject obj in objetos)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
    public void DesactivarObjetosAnterior(PisoActivado piso)
    {
        // Desactiva los objetos correspondientes al piso actual
        switch (piso)
        {
            case PisoActivado.Superior:
                DesactivarObjetos(objetosSuperior);
                break;
            case PisoActivado.Inferior:
                DesactivarObjetos(objetosInferior);
                break;
            case PisoActivado.Afueras:
                DesactivarObjetos(objetosAfueras);
                break;
        }
    }
    public void ComienzoPiso()
    {
        DesactivarObjetos(objetosSuperior);
        DesactivarObjetos(objetosAfueras);
    }
    public void ActivarObjetos(GameObject[] objetos)
    {
        foreach (GameObject obj in objetos)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
    public void ActivarObjetosActual(PisoActivado piso)
    {
        // Desactiva los objetos correspondientes al piso actual
        switch (piso)
        {
            case PisoActivado.Superior:
                ActivarObjetos(objetosSuperior);
                break;
            case PisoActivado.Inferior:
                ActivarObjetos(objetosInferior);
                break;
            case PisoActivado.Afueras:
                ActivarObjetos(objetosAfueras);
                break;
        }
    }
    
}