using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Cinemachine;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ProgressManager progressManager;
    public GameObject pantallaVictoria;

    [Header("Fragmentos")]
    public int fragmentosTotales = 4;
    public bool WaterEffect = true;
    public bool FireEffect = true;
    public bool EarthEffect = true;
    public bool ElectricityEffect = true;

    public enum ElementoActivo { Ninguno, Fuego, Agua, Electricidad, Tierra }
    public ElementoActivo elementoBoss = ElementoActivo.Ninguno;

    public enum EstadoJuego { Jugando, Pausado, Ganado, Perdido }
    public EstadoJuego estadoActual = EstadoJuego.Jugando;

    public enum PisoActivado { Superior, Inferior, Afueras }
    public PisoActivado PisoActual = PisoActivado.Afueras;

    public GameObject[] objetosSuperior;
    public GameObject[] objetosInferior;
    public GameObject[] objetosAfueras;
    public GameObject[] objetosPagina1;
    public GameObject[] objetosPagina2;
    public GameObject[] objetosPagina3;
    public GameObject[] objetosPagina4;
    public GameObject[] objetosPagina5;
    public GameObject[] objetosPagina6;
    public GameObject[] objetosPagina7;
    public GameObject paredBoss1;
    public GameObject paredBoss2;

    public AudioSource musicaFondo;
    public AudioClip musicaNormal;
    public AudioClip musicaBoss;

    public PlayerController jugador;
    public bool jugadorMelee = true;

    public UnityEvent onFragmentoRecolectado;
    public UnityEvent onTodosFragmentosRecolectados;
    public UnityEvent onBossDerrotado;

    public GameObject meleePrefab;
    public GameObject ArqueroPrefab;
    public CinemachineVirtualCamera virtualCam;

    public Transform puntoSpawn;
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

    private void Start()
    {
        ComienzoPiso();
        pantallaVictoria.SetActive(false);
        Debug.Log("GameManager ha iniciado.");
        int personajeSeleccionado = PlayerPrefs.GetInt("PersonajeSeleccionado");
        Debug.Log("Instanciando personaje: " + personajeSeleccionado);

        GameObject jugador;
        if (personajeSeleccionado == 1)
        {
            jugador = Instantiate(meleePrefab, puntoSpawn.position, Quaternion.identity);
        }
        else
        {
            jugador = Instantiate(ArqueroPrefab, puntoSpawn.position, Quaternion.identity);
        }

        virtualCam.Follow = jugador.transform;
        virtualCam.LookAt = jugador.transform;
        if (musicaFondo != null && musicaNormal != null)
        {
            CambiarMusica(musicaNormal);
        }
    }
    public void ColocarFragmento(string tipo)
    {
        switch (tipo.ToLower())
        {
            case "water":
                WaterEffect = false;
                progressManager.ColocarElemento("agua");
                progressManager.fragmentosColocados++;
                SumarPorcentaje(6);

                break;
            case "fire":
                FireEffect = false;
                progressManager.ColocarElemento("fuego");
                progressManager.fragmentosColocados++;
                SumarPorcentaje(6);
                break;
            case "earth":
                EarthEffect = false;
                progressManager.ColocarElemento("tierra");
                progressManager.fragmentosColocados++;
                SumarPorcentaje(6);
                break;
            case "electricity":
                ElectricityEffect = false;
                progressManager.ColocarElemento("electricidad");
                progressManager.fragmentosColocados++;
                SumarPorcentaje(6);
                break;
            default:
                Debug.LogError("Fragmento no reconocido: " + tipo);
                return;
        }
    }

    public void RecolectarFragmento(string tipo)
    {
        switch (tipo.ToLower())
        {
            case "water":
                ProgressManager.Instance.DesbloquearElemento("Agua");
                progressManager.fragmentosRecolectados++;
                SumarPorcentaje(10);

                break;
            case "fire":
                ProgressManager.Instance.DesbloquearElemento("Fuego");
                progressManager.fragmentosRecolectados++;
                SumarPorcentaje(10);
                break;
            case "earth":
                ProgressManager.Instance.DesbloquearElemento("Tierra");
                progressManager.fragmentosRecolectados++;
                SumarPorcentaje(10);
                break;
            case "electricity":
                ProgressManager.Instance.DesbloquearElemento("Electricidad");
                progressManager.fragmentosRecolectados++;
                SumarPorcentaje(10);
                break;
            default:
                Debug.LogError("Fragmento no reconocido: " + tipo);
                return;
        }
    }

    public void CambiarEstado(EstadoJuego nuevoEstado)
    {
        estadoActual = nuevoEstado;
        Time.timeScale = (estadoActual == EstadoJuego.Pausado) ? 0 : 1;
    }

    public void CambiarElementoBoss(ElementoActivo nuevoElemento)
    {
        elementoBoss = nuevoElemento;
        Debug.Log("Elemento del Boss cambiado a: " + elementoBoss);
    }

    public void RestarVidaJugador(int cantidad)
    {
        if (jugador != null)
        {
            jugador.TakeDamage(cantidad);
        }
    }

    public void CambiarMusica(AudioClip nuevaMusica)
    {
        if (musicaFondo == null || nuevaMusica == null) return;

        musicaFondo.clip = nuevaMusica;
        musicaFondo.Play();
    }

    public void BossDerrotado()
    {
        SumarPorcentaje(4);
        CambiarEstado(EstadoJuego.Ganado);
        onBossDerrotado?.Invoke();
        Debug.Log("¡Boss derrotado!");
        pantallaVictoria.SetActive(true);
    }

    public void DesactivarObjetos(GameObject[] objetos)
    {
        foreach (GameObject obj in objetos)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    public void BorrarObjetos(GameObject[] objetos)
    {
        foreach (GameObject obj in objetos)
        {
            if (obj != null)
                Destroy(obj);
        }
    }

    public void DesactivarObjetosAnterior(PisoActivado piso)
    {
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
        DesactivarObjetos(objetosInferior);
        paredBoss1.SetActive(false);
        paredBoss2.SetActive(false);
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

    public void RecogerPagina(int numeroPagina)
    {
        ProgressManager.Instance.RecogerPagina(numeroPagina);

        switch (numeroPagina)
        {
            case 1: BorrarObjetos(objetosPagina1); break;
            case 2: BorrarObjetos(objetosPagina2); break;
            case 3: BorrarObjetos(objetosPagina3); break;
            case 4: BorrarObjetos(objetosPagina4); break;
            case 5: BorrarObjetos(objetosPagina5); break;
            case 6: BorrarObjetos(objetosPagina6); break;
            case 7: BorrarObjetos(objetosPagina7); break;
            default:
                Debug.LogWarning("Número de página no válido: " + numeroPagina);
                break;
        }
    }
    public void SumarPorcentaje(int valor)
    {
        progressManager.porcentajeDeJuego = progressManager.porcentajeDeJuego + valor;
    }
}