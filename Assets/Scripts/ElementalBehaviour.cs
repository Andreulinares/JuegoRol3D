using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
//using UnityEditor.Profiling.Memory.Experimental;
using StarterAssets;
using UnityEngine.UI;

public class ElementalBehaviour : MonoBehaviour
{
    private Animator animator;

    public int vidaMaxima = 30;
    public int vidaActual;
    private NavMeshAgent agent;
    private GameObject jugador;
    public EnemigoMelee enemigo;
    public PlayerController playerController;
    public GameObject proyectil;
    public Transform puntoDisparo;
    public float velocidadProyectil = 10f;
    public float cooldownDisparo = 1.5f;
    private float cooldownUltimoDisparo = 0f;
    public bool isStunned = false;
    private float stunTimer = 0f;



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

    public Image barraVidaElemental;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ElementalPatrolScript = GetComponent<ElementalPatrol>();
        AsignadorDeTipos = GetComponent<AsignarTipo>();
        jugador = GameObject.FindWithTag("Player");

        vidaActual = vidaMaxima;

        animator = GetComponent<Animator>();

        ActualizarBarraVida();
    }

    // Update is called once per frame
    void Update()
    {

        if (jugador == null)
        {
            jugador = GameObject.FindWithTag("Player");
        }

        if (!isAlive)
        {
            Muerte();
        }else{
            if (isStunned)
            {
                inStun();
                return;
            }
            /*HayEnemigoMeleeCerca = ComprobarEnemigosMelee();
            HayJugadorCerca = ComprobarJugador();*/

            if (enemigoDetectado)
            {
                //HayEnemigoMeleeEnArea = ComprobarAreaInfluencia();
                if (!enemigoEstaEnAreaInfluencia){
                    DetenerPatrullaje();
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

    public void TakeDamage(int cantidad){
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        ActualizarBarraVida();
        animator.SetTrigger("isKnockback");

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
    }

    void Muerte(){
        animator.SetTrigger("isDead");
        animator.SetBool("isAttack", false);
        //Destroy(gameObject, 3f);
    }

    public void DestroyObject(){
        Destroy(gameObject);
    }

    //Asignar tipo al enemigo
    void AsignarTipo(){
        AsignadorDeTipos.AsignarTipoElemental(enemigo);
    }

    //Acercarse al jugador para que este en el rango de ataque para atacar a distancia
    void AcercarseAlJugador(){
        Vector3 direccionJugador = (jugador.transform.position - transform.position).normalized;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionJugador);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f);
        agent.SetDestination(jugador.transform.position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttack", false);
        Debug.Log("Moviendose hacia el jugador");
    }

    void AtacarDistancia(){
        Vector3 direccionJugador = (jugador.transform.position - transform.position).normalized;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionJugador);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
        if (Time.time >= cooldownUltimoDisparo + cooldownDisparo)
        {

            animator.SetBool("isAttack", true);
            animator.SetBool("isWalking", false);
            cooldownUltimoDisparo = Time.time;
        }
    }

    public void DispararProyectil()
{
    if (proyectil != null && puntoDisparo != null)
    {
        Vector3 direccion = (jugador.transform.position - puntoDisparo.position).normalized;
        GameObject nuevoProyectil = Instantiate(proyectil, puntoDisparo.position, Quaternion.LookRotation(direccion));

        Rigidbody rb = nuevoProyectil.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direccion * velocidadProyectil;
        }
    }
}

    //Patrullar
    void Patrullar()
    {
        ElementalPatrolScript.ActivarPatrullaje();
        float speed = agent.velocity.magnitude;

        bool isMoving = speed > 0.1f && agent.remainingDistance > 0.5f;
        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isAttack", false);
        
        if (agent.velocity.magnitude > 0.1f)
        {
            Vector3 direccionMovimiento = agent.velocity.normalized;
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f);
        }
    }

    void DetenerPatrullaje(){
        ElementalPatrolScript.DesactivarPatrullaje();
        animator.SetBool("isWalking", false);

        ElementalPatrolScript.ghost.GetComponent<GhostRunner>().Detener();
    }

    //Llamar al enemigo melee para que se acerque al elemental para poder obtener tipo 
    void LlamarEnemigoMelee(){
        enemigo.Llamada(transform);
        Debug.Log("Llamando al enemigo melee para que se acerque.");
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
        EnemyBarra.InterfaceEnemy.ActualizarVidaEnemy(barraVidaElemental, vidaActual / vidaMaxima);
    }
}
