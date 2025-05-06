using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEditor.Profiling.Memory.Experimental;

public class ElementalBehaviour : MonoBehaviour
{

    public int vidaMaxima = 30;
    private int vidaActual;
    private NavMeshAgent agent;
    private GameObject jugador;
    public EnemigoMelee enemigo;

    public GameObject proyectil;
    public Transform puntoDisparo;
    public float velocidadProyectil = 10f;
    public float cooldownDisparo = 1.5f;
    private float cooldownUltimoDisparo = 0f;


    private bool isAlive = true;
    /*private bool HayEnemigoMeleeCerca = false;
    private bool HayJugadorCerca = false;
    private bool HayEnemigoMeleeEnArea = false; */

    public bool enemigoDetectado = false;
    public bool enemigoEstaEnAreaInfluencia = false;
    public bool jugadorDetectado = false;

    public float rangoDeAtaque = 10f;

    private ElementalPatrol ElementalPatrolScript;
    private AsignarTipo AsignadorDeTipos;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ElementalPatrolScript = GetComponent<ElementalPatrol>();
        AsignadorDeTipos = GetComponent<AsignarTipo>();
        jugador = GameObject.FindWithTag("Player");

        vidaActual = vidaMaxima;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            Muerte();
        }else{
            /*HayEnemigoMeleeCerca = ComprobarEnemigosMelee();
            HayJugadorCerca = ComprobarJugador();*/

            if (enemigoDetectado)
            {
                //HayEnemigoMeleeEnArea = ComprobarAreaInfluencia();
                if (!enemigoEstaEnAreaInfluencia){
                    LlamarEnemigoMelee();
                } else{

                    if(!EnemigoTieneTipoElemental()){
                        AsignarTipo();
                    } else if(jugadorDetectado){
                        DetenerPatrullaje();
                        if (JugadorEnRangoDeAtaque()){
                            AtacarDistancia();
                        }else{
                            AcercarseAlJugador();
                        }
                    }else{
                        Patrullar();
                    }
                }
            } else if (jugadorDetectado){
                DetenerPatrullaje();
                if (JugadorEnRangoDeAtaque()){
                AtacarDistancia();
                }else{
                    AcercarseAlJugador();
                }
            }else{
                Patrullar();
            }
        }
    }

    //Comprobar si el enemigo melee esta en el area de deteccion
    public void EnemigoDetectado(bool estado)
    {
        enemigoDetectado = estado;
    }

    //Comprobar si el jugador esta en el area de deteccion
    public void JugadorDetectado(bool estado)
    {
        jugadorDetectado = estado;
    }

    //Comprobar si el enemigo esta en el area de influencia
    public void EnemigoEnArea(bool estado)
    {
        enemigoEstaEnAreaInfluencia = estado;
    }

    //Comprobar si el enemigo tiene un tipo elemental asignado
    bool EnemigoTieneTipoElemental(){
        return enemigo.tieneTipo;
    }

    bool JugadorEnRangoDeAtaque(){
        float distancia = Vector3.Distance(transform.position, jugador.transform.position);
        return distancia <= rangoDeAtaque;
    }

    public void RecibirDaño(int cantidad){
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        if (vidaActual <= 0){
            isAlive = false;
        }else{
            isAlive = true;
        }
    }

    void Muerte(){
        //Animacion muerte
        Destroy(gameObject);
    }

    //Asignar tipo al enemigo
    void AsignarTipo(){
        AsignadorDeTipos.AsignarTipoElemental(enemigo);
    }

    //Acercarse al jugador para que este en el rango de ataque para atacar a distancia
    void AcercarseAlJugador(){
        agent.SetDestination(jugador.transform.position);
        Debug.Log("Moviendose hacia el jugador");
    }

    void AtacarDistancia(){
        if (Time.time >= cooldownUltimoDisparo + cooldownDisparo){
            if (proyectil != null && puntoDisparo != null){
                GameObject nuevoProyectil = Instantiate(proyectil, puntoDisparo.position, puntoDisparo.rotation); 

                Rigidbody rb = nuevoProyectil.GetComponent<Rigidbody>();
                if(rb != null){
                    Vector3 direccion = jugador.transform.position - puntoDisparo.position;
                    rb.velocity = direccion * velocidadProyectil;
                }
            }

            cooldownUltimoDisparo = Time.time;
        }
    }

    //Patrullar
    void Patrullar(){
        ElementalPatrolScript.ActivarPatrullaje();
    }

    void DetenerPatrullaje(){
        ElementalPatrolScript.DesactivarPatrullaje();

        ElementalPatrolScript.ghost.GetComponent<GhostRunner>().Detener();
    }

    //Llamar al enemigo melee para que se acerque al elemental para poder obtener tipo 
    void LlamarEnemigoMelee(){
        enemigo.Llamada(transform);
        Debug.Log("Llamando al enemigo melee para que se acerque.");
    }
}
