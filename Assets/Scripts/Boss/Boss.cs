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
    public GameObject aguaBossCollider;
    public GameObject tierraBossCollider;
    public GameObject fuegoBossCollider;
    public GameObject electricidadBossCollider;
    private Animator animator;
    private bool estaAtacando = false;
    private string estadoActual = "";
    public bool isAttacking = false;
    public int attackRangeClose = 3;
    public int attackRangeMedium = 5;
    public int detectionRange = 10;
    public int PVMax = 100;
    public int PVActual;
    public bool attackBuff = false;
    public int fragmentosJugador = 0;
    public bool invencibility = false;
    public Transform[] puntosPatrulla;
    public Transform puntoSpawnBoss;
    public GameObject[] auras;
    public int indiceActual = 0;
    public enum AttackType { Fire, Water, Electricity, Earth, None }
    public AttackType currentAttackType = AttackType.None;
    public PlayerController.Elemento bossDebilElemento = PlayerController.Elemento.None;
    public ArqueroController.Elemento bossDebilElementoA = ArqueroController.Elemento.None;
    private float distanceToPlayer;
    public bool isChasing = false;
    public bool isApproaching = false;
    public bool muerto = false;
    public float velocidadAgente;
    private float attackCooldown = 5.0f; // segundos entre ataques, ajusta según necesites
    private float lastAttackTime = -999f; // momento del último ataque


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
        aguaBossCollider.SetActive(false);
        tierraBossCollider.SetActive(false);
        fuegoBossCollider.SetActive(false);
        electricidadBossCollider.SetActive(false);
    }

    private void Update()
    {
        velocidadAgente = agent.speed;
        animator.SetBool("isApproaching", isApproaching);
        animator.SetBool("Muerto", muerto);

        if (muerto)
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

        if (estaAtacando)
        {
            velocidadesAtaque();
            LogicaAuras();
            isChasing = false;
        }
        else
        {
            agent.speed = 3.5f;
            foreach (GameObject aura in auras)
            {
                if (aura != null)
                {
                    aura.SetActive(false);
                }
            }
            aguaBossCollider.SetActive(false);
            tierraBossCollider.SetActive(false);
            fuegoBossCollider.SetActive(false);
            electricidadBossCollider.SetActive(false);
        }

        animator.SetBool("isChasing", isChasing);

        // Detectar fin de animación de ataque
        if (estaAtacando && state.normalizedTime >= 1f && state.IsName(estadoActual))
        {
            FinAnimacion();
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
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                if (distanceToPlayer <= attackRangeMedium && distanceToPlayer > attackRangeClose)
                {
                    AttackMediumRange();
                    lastAttackTime = Time.time;  // actualizar el tiempo del último ataque
                    isAttacking = true;
                    isChasing = false;
                }
                else if (distanceToPlayer <= attackRangeClose)
                {
                    AttackCloseRange();
                    lastAttackTime = Time.time;  // actualizar el tiempo del último ataque
                    isAttacking = true;
                    isChasing = false;
                }
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
    }

    public void TakeDamage(int pega)
    {
        int vidaVulnerable = fragmentosJugador * 25;
        int vidaMinimaPermitida = PVMax - vidaVulnerable;

        if (selectionMenu.currentSelection == 1)
        {
            if (playerController.playerAtaqueElemento == PlayerController.Elemento.None)
            {
                playerController.manaActualPlayer = Mathf.Min(playerController.manaActualPlayer + 25, 100);
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
                arqueroController.manaActualPlayer = Mathf.Min(arqueroController.manaActualPlayer + 25, 100);
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
                bossDebilElemento = PlayerController.Elemento.Earth;
                bossDebilElementoA = ArqueroController.Elemento.Earth;
                if (gameManager.ElectricityEffect)
                {
                    animator.speed = 1.5f;
                    Invoke(nameof(ActivarElectricidadBossCollider), 0.2f);
                }
                else
                {
                    Invoke(nameof(ActivarElectricidadBossCollider), 0.3f);
                }
                PerformAttack(AttackType.Electricity);
                auras[0].SetActive(true);
                
                break;
            case 1:
                bossDebilElemento = PlayerController.Elemento.Fire;
                bossDebilElementoA = ArqueroController.Elemento.Fire;
                if (gameManager.EarthEffect)
                {
                    invencibility = true;
                }
                PerformAttack(AttackType.Earth);
                auras[1].SetActive(true);
                Invoke(nameof(ActivarTierraBossCollider), 2.25f);
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
                bossDebilElemento = PlayerController.Elemento.Water;
                bossDebilElementoA = ArqueroController.Elemento.Water;
                if (gameManager.FireEffect)
                {
                    attackBuff = true;
                }
                PerformAttack(AttackType.Fire);
                ActivarFuegoBossCollider();
                break;
            case 1:
                bossDebilElemento = PlayerController.Elemento.Electricity;
                bossDebilElementoA = ArqueroController.Elemento.Electricity;
                PerformAttack(AttackType.Water);
                PVActual = Mathf.Min(PVActual + 15, PVMax);
                auras[2].SetActive(true);
                Invoke(nameof(ActivarAguaBossCollider), 0.3f);
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
                break;
            case AttackType.Water:
                EjecutarAtaque("AtaqueAgua", "AtacarAgua");
                break;
            case AttackType.Electricity:
                EjecutarAtaque("AtaqueElectricidad", "AtacarElectricidad");
                break;
            case AttackType.Earth:
                EjecutarAtaque("AtaqueTierra", "AtacarTierra");
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
        GameManager.Instance.BossDerrotado();
        gameManager.paredBoss1.SetActive(false);
        gameManager.paredBoss2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAttacking)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(10);
            }

            ArqueroController arquero = other.GetComponent<ArqueroController>();
            if (arquero != null)
            {
                arquero.TakeDamage(10);
            }
            playerController.isInvincible = true;
        }
    }

    public void EjecutarAtaque(string nombreEstado, string nombreAtaque)
    {
        animator.SetTrigger(nombreAtaque);
        estadoActual = nombreEstado;
        estaAtacando = true;
    }

    public void velocidadesAtaque()
    {
        switch (currentAttackType)
        {
            case AttackType.Water:
            case AttackType.Electricity:
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

    public void LogicaAuras()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("AtaqueFuego"))
        {
            auras[3].SetActive(true);
        }
        if (state.IsName("AtaqueAgua"))
        {
            auras[2].SetActive(true);
        }
        if (state.IsName("AtaqueElectricidad"))
        {
            auras[0].SetActive(true);
        }
        if (state.IsName("AtaqueTierra"))
        {
            auras[1].SetActive(true);
        }
    }
    public void FinAnimacion()
    {
        estaAtacando = false;
        currentAttackType = AttackType.None;
        bossDebilElemento = PlayerController.Elemento.None;
        bossDebilElementoA = ArqueroController.Elemento.None;
        invencibility = false;
        attackBuff = false;
        isAttacking = false;
        playerController.isInvincible = false;
        agent.speed = 3.5f;
        animator.speed = 1f;
        aguaBossCollider.SetActive(false);
        tierraBossCollider.SetActive(false);
        fuegoBossCollider.SetActive(false);
        electricidadBossCollider.SetActive(false);
        animator.SetTrigger("AcabaAtaque");
    }
    private void ActivarAguaBossCollider()
    {
        aguaBossCollider.SetActive(true);
    }
    private void ActivarTierraBossCollider()
    {
        tierraBossCollider.SetActive(true);
    }
    private void ActivarFuegoBossCollider()
    {
        fuegoBossCollider.SetActive(true);
    }
    private void ActivarElectricidadBossCollider()
    {
        electricidadBossCollider.SetActive(true);
    }
}
