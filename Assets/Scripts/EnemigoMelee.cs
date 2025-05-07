using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using StarterAssets;

public class EnemigoMelee : MonoBehaviour
{
    public GameObject[] auraPrefabs; //Array con las auras
    public PlayerController playerController;
    //public Transform puntoAura;
    private GameObject aura;
    public int vida = 50;
    public bool tieneTipo = false; 
    private bool transformado = true;
    private bool EstoySiendoLlamado = false;
    private bool isAlive = true;
    private bool EstoyConvertido = false;
    private bool EstoyViendoJugador = false;
    private bool EstoyEnAreaInfluencia = false;

    private bool EstoyDentro = false; 

    public float rangoDeAtaque = 2.5f;
    public bool isStunned = false;
    private float stunTimer = 0f;

    public AsignarTipo.TipoElemental tipoActual;
    public PlayerController.Elemento meleeDebilElemento = PlayerController.Elemento.None;
    public Transform enemigoElemental;

    private NavMeshAgent agent;
    private GameObject jugador;

    private MeleePatrol PatrolMelee;
    private PlayerDetection playerDetection;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PatrolMelee = GetComponent<MeleePatrol>();
        jugador = GameObject.FindWithTag("Player");
        playerDetection = GetComponentInChildren<PlayerDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive){
            Muerte();
        }
        else
        {
            if (isStunned)
            {
                inStun();
                return;
            }
            

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
                        DetenerPatrullaje();
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
                    DetenerPatrullaje();
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
                aura = auraPrefabs[0];
                meleeDebilElemento=PlayerController.Elemento.Water;
                break;
            case AsignarTipo.TipoElemental.Agua:
                GetComponent<Renderer>().material.color = Color.blue;
                aura = auraPrefabs[1];
                meleeDebilElemento=PlayerController.Elemento.Electricity;
                break;
            case AsignarTipo.TipoElemental.Tierra:
                GetComponent<Renderer>().material.color = Color.green;
                aura = auraPrefabs[2];
                meleeDebilElemento=PlayerController.Elemento.Fire;
                break;
            case AsignarTipo.TipoElemental.Electricidad:
                GetComponent<Renderer>().material.color = Color.yellow;
                aura = auraPrefabs[3];
                meleeDebilElemento=PlayerController.Elemento.Earth;
                break;
        }
        if (aura != null)
        {
            GameObject auraInstanciada = Instantiate(aura, transform.position, Quaternion.identity);
            auraInstanciada.transform.parent = transform; 
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
        if (playerDetection.jugadorDetectado){
            return true;
        }else{
            return false;
        }
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
        Debug.Log("Golpeando al jugador");
    }

    void Patrullar(){
        PatrolMelee.ActivarPatrullaje();
    }

    void DetenerPatrullaje(){
        PatrolMelee.DesactivarPatrullaje();

        PatrolMelee.ghost.GetComponent<GhostMeleeRunner>().Detener();
    }
    public void TakeDamage(int pega)
    {
        
        if (transformado == false)
        {

        }
        else if (playerController.playerAtaqueElemento == meleeDebilElemento)
        {
            pega += 4;
            Debug.Log("¡Daño crítico! El ataque fue efectivo contra la debilidad del melee.");
        }

        int nuevaVida = Mathf.Max(vida - pega);

        int pegaReal = vida - nuevaVida;
        vida = nuevaVida;

        if (vida <= 0){
            isAlive = false;
            playerController.manaActualPlayer=playerController.manaActualPlayer+25;
            if(playerController.manaActualPlayer>=100)
            {
                playerController.manaActualPlayer=100;
            }
        }else{
            //Animacion stun
            Stun(2);
            isAlive = true;
        }

        Debug.Log("Se hizo " + pegaReal + " de daño. Vida actual: " + vida);
    }
    public void Stun(float stunDuration)
    {
        //AnimacionStun
        isStunned = true;
        stunTimer = stunDuration;
    }
    public void inStun()
    {
        stunTimer -= Time.deltaTime;
                if (stunTimer <= 0f)
                {
                    isStunned = false;
                }
    }
}
