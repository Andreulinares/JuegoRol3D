using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ElementalBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject jugador;
    public EnemigoMelee enemigo;

    public GameObject proyectil;
    public Transform puntoDisparo;
    public float velocidadProyectil = 20f;


    private bool isAlive = true;
    private bool HayEnemigoMeleeCerca = false;
    private bool HayJugadorCerca = false;
    private bool HayEnemigoMeleeEnArea = false; 

    public float rangoDeAtaque = 10f;

    private ElementalPatrol ElementalPatrolScript;
    private AsignarTipo AsignadorDeTipos;
    private MeleeDetection meleeDetection;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ElementalPatrolScript = GetComponent<ElementalPatrol>();
        AsignadorDeTipos = GetComponent<AsignarTipo>();
        meleeDetection = GetComponent<MeleeDetection>();
        jugador = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            Muerte();
        }else{
            HayEnemigoMeleeCerca = ComprobarEnemigosMelee();
            HayJugadorCerca = ComprobarJugador();
        }

        if (HayEnemigoMeleeCerca)
        {
            HayEnemigoMeleeEnArea = ComprobarAreaInfluencia();
            if (!HayEnemigoMeleeEnArea){
                LlamarEnemigoMelee();
            } else{

                if(!EnemigoTieneTipoElemental()){
                    AsignarTipo();
                } else if(HayJugadorCerca){
                    if (JugadorEnRangoDeAtaque()){
                        AtacarDistancia();
                    }else{
                        AcercarseAlJugador();
                    }
                }
            }
        }
        else if (HayJugadorCerca)
        {
            if (JugadorEnRangoDeAtaque()){
                AtacarDistancia();
            }else{
                AcercarseAlJugador();
            }
        }
        else
        {
            Patrullar();
        }
    }

    //Comprobar si el enemigo melee esta en el area de deteccion
    bool ComprobarEnemigosMelee(){
        if (meleeDetection.enemigoDetectado){
            return true;
        }else{
            return false;
        }
    }

    //Comprobar si el jugador esta en el area de deteccion
    bool ComprobarJugador(){
        if (meleeDetection.jugadorDetectado){
            return true;
        }else{
            return false;
        }
    }

    //Comprobar si el enemigo esta en el area de influencia
    bool ComprobarAreaInfluencia(){
        if (meleeDetection.enemigoEstaEnAreaInfluencia){
            return true;
        }else{
            return false;
        }
    }

    //Comprobar si el enemigo tiene un tipo elemental asignado
    bool EnemigoTieneTipoElemental(){
        return enemigo.tieneTipo;
    }

    bool JugadorEnRangoDeAtaque(){
        float distancia = Vector3.Distance(transform.position, jugador.transform.position);
        return distancia <= rangoDeAtaque;
    }

    void Muerte(){

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
        if (proyectil != null && puntoDisparo != null){
            GameObject nuevoProyectil = Instantiate(proyectil, puntoDisparo.position, puntoDisparo.rotation); 

            Rigidbody rb = nuevoProyectil.GetComponent<Rigidbody>();
            if(rb != null){
                Vector3 direccion = jugador.transform.position - puntoDisparo.position;
                rb.velocity = direccion * velocidadProyectil;
            }
        }
    }

    //Patrullar
    void Patrullar(){
        ElementalPatrolScript.ActivarPatrullaje();
    }

    //Llamar al enemigo melee para que se acerque al elemental para poder obtener tipo 
    void LlamarEnemigoMelee(){
        enemigo.Llamada(transform);
        Debug.Log("Llamando al enemigo melee para que se acerque.");
    }
}
