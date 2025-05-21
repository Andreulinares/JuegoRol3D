using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public GameManager gameManager;
    public ProgressManager progressManager;
    private GameObject jugador;
    public SelectionMenu selectionMenu;
    public NavMeshAgent agent;
    public PlayerController playerController;
    public ArqueroController arqueroController;
    private Animator animator;
    private bool estaAtacando = false;
    private string estadoActual = "";
    private bool isAttacking = false;
    public int attackRangeClose = 2;
    public int attackRangeMedium = 5;
    public int detectionRange = 10;
    public int PVMax = 100;
    public int PVActual;
    public bool attackBuff = false;
    public int fragmentosJugador = 0;
    public bool invencibility = false;
    public Transform[] puntosPatrulla;
    public Transform puntoSpawnBoss;
    public int indiceActual = 0;
    public enum AttackType { Fire, Water, Electricity, Earth, None }
    public AttackType currentAttackType = AttackType.None;
    public PlayerController.Elemento bossDebilElemento = PlayerController.Elemento.None;
    public ArqueroController.Elemento bossDebilElementoA = ArqueroController.Elemento.None;
    private float distanceToPlayer;
    public bool isChasing = false;
    public bool isApproaching = false;
    public bool muerto = false;
    private float velocidadOriginal;
    public float velocidadAgente;


    private void Start()
    {
        animator = GetComponent<Animator>();
        if (jugador == null)
        {
            jugador = GameObject.FindWithTag("Player");
        }
        PVActual = PVMax;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        transform.position = puntoSpawnBoss.position;
        estaAtacando = false;
    }

    private void Update()
    {
        velocidadAgente = agent.speed;
        Debug.Log(velocidadAgente);
        animator.SetBool("isChasing", isChasing);
        animator.SetBool("isApproaching", isApproaching);
        animator.SetBool("Muerto", muerto);

        if (muerto == true)
        {
            return;
        }
        if (PVActual == 0)
        {
            Muerte();
            return;
        }

        fragmentosJugador = progressManager.fragmentosColocados;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (estaAtacando == true)
        {
            velocidadesAtaque();
        }
        else
        {
            agent.speed = 3.5f;
        }

        // Si estamos atacando y ya terminó la animación
        Debug.Log(estaAtacando);
        Debug.Log(state.normalizedTime);
        Debug.Log(estadoActual);
        Debug.Log(state.IsName(estadoActual));
        if (estaAtacando && state.normalizedTime >= 1f && state.IsName(estadoActual))
        {
            Debug.Log($"Ataque '{estadoActual}' finalizado.");
            estaAtacando = false;
            currentAttackType = AttackType.None;
            bossDebilElemento = PlayerController.Elemento.None;
            bossDebilElementoA = ArqueroController.Elemento.None;
            invencibility = false;
            attackBuff = false;
            isAttacking = false;
            agent.speed = 3.5f;
            animator.speed = 1f;
            animator.SetTrigger("AcabaAtaque");
            return;
        }

        distanceToPlayer = Vector3.Distance(transform.position, jugador.transform.position);

        if (isApproaching && distanceToPlayer <= attackRangeClose)
        {
            isApproaching = false;
        }

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            ChasePlayer();
            isAttacking = false;
            animator.SetBool("Patrol", false);
        }
        else
        {
            isChasing = false;
            Patrol();
            isAttacking = false;
        }

        if (isChasing && !isApproaching)
        {
            if (distanceToPlayer <= attackRangeMedium && distanceToPlayer > attackRangeClose)
            {
                AttackMediumRange();
                isAttacking = true;
            }
            else if (distanceToPlayer <= attackRangeClose)
            {
                AttackCloseRange();
                isAttacking = true;
            }
        }
    }
    public void ActualizarFragmentos(int cantidad)
    {
        fragmentosJugador = Mathf.Clamp(cantidad, 0, 4); // máximo 4 fragmentos
    }

    public void Patrol()
    {
        if (puntosPatrulla.Length == 0) return;

        agent.SetDestination(puntosPatrulla[indiceActual].position);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            indiceActual = (indiceActual + 1) % puntosPatrulla.Length;
        }
        animator.SetBool("Patrol", true);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(jugador.transform.position);

        gameManager.paredBoss1.SetActive(true);
        gameManager.paredBoss2.SetActive(true);
        Debug.Log("persiguiendo");
    }

    public void TakeDamage(int pega)
    {
        int vidaVulnerable = fragmentosJugador * 25;
        int vidaMinimaPermitida = PVMax - vidaVulnerable;
        if (selectionMenu.currentSelection == 1)
        {
            if (playerController.playerAtaqueElemento == PlayerController.Elemento.None)
            {
                playerController.manaActualPlayer = playerController.manaActualPlayer + 25;
                if (playerController.manaActualPlayer >= 100)
                {
                    playerController.manaActualPlayer = 100;
                }
            }

            if (currentAttackType == AttackType.None)
            {
                Debug.Log("Ataque normal");
            }
            else if (playerController.playerAtaqueElemento == bossDebilElemento)
            {
                pega += 4;
                Debug.Log("¡Daño crítico! El ataque fue efectivo contra la debilidad del boss.");
            }
            else
            {
                Debug.Log("Ataque elemental NO crítico");
            }
        }
        else if (selectionMenu.currentSelection == 2)
        {
            if (arqueroController.playerAtaqueElemento == ArqueroController.Elemento.None)
            {
                arqueroController.manaActualPlayer = arqueroController.manaActualPlayer + 25;
                if (arqueroController.manaActualPlayer >= 100)
                {
                    arqueroController.manaActualPlayer = 100;
                }
            }

            if (currentAttackType == AttackType.None)
            {
                Debug.Log("Ataque normal");
            }
            else if (arqueroController.playerAtaqueElemento == bossDebilElementoA)
            {
                pega += 4;
                Debug.Log("¡Daño crítico! El ataque fue efectivo contra la debilidad del boss.");
            }
            else
            {
                Debug.Log("Ataque elemental NO crítico");
            }
        }
        int nuevaVida = Mathf.Max(PVActual - pega, vidaMinimaPermitida);

        int pegaReal = PVActual - nuevaVida;
        PVActual = nuevaVida;

        Debug.Log("Se hizo " + pegaReal + " de daño. Vida actual: " + PVActual);
    }

    private void AttackMediumRange()
    {
        int attackChoice = Random.Range(0, 3);
        switch (attackChoice)
        {
            case 0:
                if (gameManager.ElectricityEffect == true)
                {
                    bossDebilElemento = PlayerController.Elemento.Earth;
                    bossDebilElementoA = ArqueroController.Elemento.Earth;
                    animator.speed =1.5f;
                    PerformAttack(AttackType.Electricity);
                }
                else
                {
                    bossDebilElemento = PlayerController.Elemento.Earth;
                    bossDebilElementoA = ArqueroController.Elemento.Earth;
                    PerformAttack(AttackType.Electricity);
                }
                break;
            case 1:
                if (gameManager.EarthEffect == true)
                {
                    bossDebilElemento = PlayerController.Elemento.Fire;
                    bossDebilElementoA = ArqueroController.Elemento.Fire;
                    invencibility = true;
                    PerformAttack(AttackType.Earth);
                }
                else
                {
                    bossDebilElemento = PlayerController.Elemento.Fire;
                    bossDebilElementoA = ArqueroController.Elemento.Fire;
                    PerformAttack(AttackType.Earth);
                }
                break;
            case 2:
                isApproaching = true;
                agent.SetDestination(jugador.transform.position);
                break;
        }
    }

    private void AttackCloseRange()
    {
        int attackChoice = Random.Range(0, 2);
        switch (attackChoice)
        {
            case 0:
                if (gameManager.FireEffect == true)
                {
                    bossDebilElemento = PlayerController.Elemento.Water;
                    bossDebilElementoA = ArqueroController.Elemento.Water;
                    attackBuff = true;
                    PerformAttack(AttackType.Fire);
                }
                else
                {
                    bossDebilElemento = PlayerController.Elemento.Water;
                    bossDebilElementoA = ArqueroController.Elemento.Water;
                    PerformAttack(AttackType.Fire);
                }
                break;
            case 1:
                if (gameManager.WaterEffect == true)
                {
                    bossDebilElemento = PlayerController.Elemento.Electricity;
                    bossDebilElementoA = ArqueroController.Elemento.Electricity;
                    PerformAttack(AttackType.Water);
                    PVActual = Mathf.Min(PVActual + 15, PVMax);
                }
                else
                {
                    bossDebilElementoA = ArqueroController.Elemento.Electricity;
                    PerformAttack(AttackType.Water);
                }
                break;
        }
    }

    private void PerformAttack(AttackType attack)
    {
        currentAttackType = attack;

        switch (currentAttackType)
        {
            case AttackType.Fire:
                EjecutarAtaque("AtaqueFuego", "AtacarFuego");
                Debug.Log("Boss realiza un ataque de Fuego!");
                break;
            case AttackType.Water:
                EjecutarAtaque("AtaqueAgua", "AtacarAgua");
                Debug.Log("Boss realiza un ataque de Agua!");
                break;
            case AttackType.Electricity:
                EjecutarAtaque("AtaqueElectricidad", "AtacarElectricidad");
                Debug.Log("Boss realiza un ataque de Electricidad!");
                break;
            case AttackType.Earth:
                EjecutarAtaque("AtaqueTierra", "AtacarTierra");
                Debug.Log("Boss realiza un ataque de Tierra!");
                break;
            case AttackType.None:
                break;
        }
    }

    private void Muerte()
    {
        agent.isStopped = true;
        agent.enabled = false;
        muerto = true;
        //animator.SetTrigger("Muerte");
        //animator.speed = 0f;
        GameManager.Instance.BossDerrotado();
        gameManager.paredBoss1.SetActive(false);
        gameManager.paredBoss2.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAttacking == true)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(10);
                Debug.Log("Golpe impacto a player!");
            }

            ArqueroController arquero = other.GetComponent<ArqueroController>();
            if (arquero != null)
            {
                arquero.TakeDamage(10);
                Debug.Log("Golpe impactó a arquero!");
            }
            isAttacking = false;
        }
    }
    void EjecutarAtaque(string nombreEstado, string nombreAtaque)
    {
        animator.SetTrigger(nombreAtaque);
        Debug.Log(nombreAtaque);
        estadoActual = nombreEstado;
        estaAtacando = true;
    }
    void velocidadesAtaque()
    {
        switch (currentAttackType)
    {
        case AttackType.Water:
            agent.speed = 0f;
            break;
        case AttackType.Electricity:
            agent.speed = 0f;
            break;
        case AttackType.Earth:
            agent.speed = 0f;
            break;
        case AttackType.Fire:
            agent.speed = 3.5f * 0.5f;
            break;
        default:
            agent.speed = 3.5f;
            break;
    }
    }
}

