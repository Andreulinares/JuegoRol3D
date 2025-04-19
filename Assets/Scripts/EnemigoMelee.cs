using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemigoMelee : MonoBehaviour
{
    public bool tieneTipo = false; 
    private bool transformado = false;
    private bool EstoySiendoLlamado = false;
    private bool isAlive = true;
    private bool EstoyConvertido = false;
    private bool EstoyViendoJugador = false;
    private bool EstoyEnAreaInfluencia = false;

    private bool EstoyDentro = false; 
    public bool jugadorDetectado = false;

    public float rangoDeAtaque = 2.5f;

    public AsignarTipo.TipoElemental tipoActual;
    public Transform enemigoElemental;

    private NavMeshAgent agent;
    private GameObject jugador;

    private MeleePatrol PatrolMelee;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PatrolMelee = GetComponent<MeleePatrol>();
        jugador = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive){
            Muerte();
        }else{
            EstoyConvertido = ComprobarConversion();
            EstoyViendoJugador = ComprobarDeteccionJugador();

            if (!EstoyConvertido){
                if(EstoySiendoLlamado){
                    EstoyEnAreaInfluencia = ComprobarAreaConversion();
                    if(EstoyEnAreaInfluencia){
                        AplicarTransformacion();
                    }else{
                        MoverHaciaElemental(enemigoElemental.position);
                    }
                }else{
                    if(EstoyViendoJugador){
                        if(JugadorEnRangoDeAtaque()){
                            Atacar();
                        }else{
                            AcercarseAlJugador();
                        }
                    }else{
                        Patrullar();
                    }
                }
            }else if(EstoyConvertido){
                if(EstoyViendoJugador){
                    if(JugadorEnRangoDeAtaque()){
                        Atacar();
                    }else{
                        AcercarseAlJugador();
                    }
                }else{
                    Patrullar(); 
                }
            }
        }
    }
    
    public void ObtenerTipoElemental(AsignarTipo.TipoElemental tipo)
    {
        tipoActual = tipo;
        tieneTipo = true;

        Debug.Log("Enemigo melee ha recibido tipo elemental: " + tipo);
    }

    void AplicarTransformacion()
    {
        transformado = true;

        switch (tipoActual)
        {
            case AsignarTipo.TipoElemental.Fuego:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case AsignarTipo.TipoElemental.Agua:
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            case AsignarTipo.TipoElemental.Tierra:
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case AsignarTipo.TipoElemental.Electricidad:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
        }
        Debug.Log("Transformación aplicada correctamente");
    }

    public void Llamada(Transform elemental){
        enemigoElemental = elemental;
        EstoySiendoLlamado = true;
    }

    public void MoverHaciaElemental(Vector3 enemigoElemental)
    {
        if (agent != null)
        {
            agent.SetDestination(enemigoElemental);
            Debug.Log("Enemigo melee se está moviendo hacia el elemental");
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            if(gameObject.name == "DetectarJugador"){
                jugadorDetectado = true;
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            if(gameObject.name == "DetectarJugador"){
                jugadorDetectado = false;
            }
        }
    }

    bool ComprobarConversion(){
        return transformado;
    }

    public void NotificarEstadoArea(bool estaEnArea)
    {
        EstoyDentro = estaEnArea;
    }

    bool ComprobarAreaConversion(){
        return EstoyDentro;
    }

    bool ComprobarDeteccionJugador(){
        return jugadorDetectado;
    }

    bool JugadorEnRangoDeAtaque(){
        float distancia = Vector3.Distance(transform.position, jugador.transform.position);
        return distancia <= rangoDeAtaque;
    }

    void Muerte(){

    }

    void AcercarseAlJugador(){
        agent.SetDestination(jugador.transform.position);
        Debug.Log("Moviendose hacia el jugador");
    }

    void Atacar(){
        //Atacando al jugador
    }

    void Patrullar(){
        PatrolMelee.ActivarPatrullaje();
    }
}
