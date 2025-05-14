using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class EnemigoMelee : MonoBehaviour
{
    private Animator animator;

    public GameObject[] auraPrefabs; //Array con las auras
    public PlayerController playerController;
    //public Transform puntoAura;
    private GameObject aura;
    public float vidaMaxima = 50;
    public float vidaActual;
    public bool damageBufo = false;
    public bool resistenciaBufo = false;
    public bool curarBufo = false;
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

    private PlayerController controllerPlayer;
    public Transform enemigoElemental;

    private NavMeshAgent agent;
    private GameObject jugador;

    private MeleePatrol PatrolMelee;
    private PlayerDetection playerDetection;

    public Image barraVidaMelee;

    private Renderer render;
    private Color colorOriginal;

    private float tiempoRojo = 0f;
    private bool enModoDaño = false;
    // Start is called before the first frame update
    void Start()
    {
        vidaActual=vidaMaxima;
        agent = GetComponent<NavMeshAgent>();
        PatrolMelee = GetComponent<MeleePatrol>();
        jugador = GameObject.FindWithTag("Player");
        playerDetection = GetComponentInChildren<PlayerDetection>();
        animator = GetComponent<Animator>();
        controllerPlayer = jugador.GetComponent<PlayerController>();

        render = GetComponent<Renderer>();
        colorOriginal = render.material.color;

        ActualizarBarraVida();
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

        /*if (enModoDaño)
        {
            tiempoRojo -= Time.deltaTime; 
            if (tiempoRojo <= 0f)
            {
                render.material.color = colorOriginal; 
                enModoDaño = false; 
            }
        }*/
    }

    public void ActivarEfectoDaño()
{
    render.material.color = Color.red; // Cambia el color a rojo
    tiempoRojo = 0.2f; // Define cuánto tiempo durará el efecto (0.2 segundos)
    enModoDaño = true; // Activa la lógica en `Update()`
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
                damageBufo=true;
                meleeDebilElemento=PlayerController.Elemento.Water;
                break;
            case AsignarTipo.TipoElemental.Agua:
                GetComponent<Renderer>().material.color = Color.blue;
                aura = auraPrefabs[1];
                curarBufo=true;
                meleeDebilElemento=PlayerController.Elemento.Electricity;
                break;
            case AsignarTipo.TipoElemental.Tierra:
                GetComponent<Renderer>().material.color = Color.green;
                aura = auraPrefabs[2];
                resistenciaBufo=true;
                meleeDebilElemento=PlayerController.Elemento.Fire;
                break;
            case AsignarTipo.TipoElemental.Electricidad:
                GetComponent<Renderer>().material.color = Color.yellow;
                aura = auraPrefabs[3];
                agent.speed = 6f;
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
            animator.SetBool("isWalking", false);
            animator.SetBool("isRun", true);
            animator.SetBool("isAttack", false);
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
        Destroy(gameObject);
    }

    void AcercarseAlJugador(){
        agent.SetDestination(jugador.transform.position);
        animator.SetBool("isRun", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isWalking", false);
        Debug.Log("Moviendose hacia el jugador");
    }

    void Atacar()
    {
        //Atacando al jugador
        if(curarBufo==true)
        {
            vidaActual=vidaActual+15;
            if(vidaActual>=100)
            {
                vidaActual=100;
            }
        }
        animator.SetBool("isAttack", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRun", false);
        Debug.Log("Golpeando al jugador");
    }

    void Patrullar(){
        PatrolMelee.ActivarPatrullaje();
        animator.SetBool("isWalking", true);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack", false);
    }

    void DetenerPatrullaje(){
        PatrolMelee.DesactivarPatrullaje();
        animator.SetBool("isWalking", false);
        PatrolMelee.ghost.GetComponent<GhostMeleeRunner>().Detener();
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ataque"){
            int daño = controllerPlayer.sphereDamage;
            TakeDamage(daño);
        }
    }*/
    public void TakeDamage(int pega)
    {
        float nuevaVida;
        
        if (transformado == false)
        {

        }
        else if (playerController.playerAtaqueElemento == meleeDebilElemento)
        {
            pega += 4;
            Debug.Log("¡Daño crítico! El ataque fue efectivo contra la debilidad del melee.");
        }
        if(resistenciaBufo==true)
        {
            nuevaVida = Mathf.Max(vidaActual - Mathf.RoundToInt(pega * 0.8f));
        }
        else
        {
            nuevaVida = Mathf.Max(vidaActual - pega);
        }

        float pegaReal = vidaActual - nuevaVida;
        vidaActual = nuevaVida;
        //ActivarEfectoDaño();
        ActualizarBarraVida();

        if (vidaActual <= 0){
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

        Debug.Log("Se hizo " + pegaReal + " de daño. Vida actual: " + vidaActual);
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

    void ActualizarBarraVida(){
        float porcentajeVida = vidaActual / vidaMaxima;
        EnemyBarra.InterfaceEnemy.ActualizarVidaEnemy(barraVidaMelee, porcentajeVida);
    }
}
