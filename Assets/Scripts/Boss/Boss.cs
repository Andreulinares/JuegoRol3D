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
    public Transform player;
    public NavMeshAgent agent;
    public PlayerController playerController;
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
    private float distanceToPlayer;
    public bool isChasing = false;
    private bool isInCooldown = false;
    public bool isApproaching = false;
    public float cooldownTiempo = 2f;
    private float cooldownTimer = 0f;
    public bool muerto = false;

    private void Start()
    {
        PVActual = PVMax;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        transform.position=puntoSpawnBoss.position;
    }

    private void Update()
    {
        if (muerto ==true)
        {
            return;
        }
        if (PVActual == 0)
        {
            Muerte();
            return;
        }

        fragmentosJugador=progressManager.fragmentosColocados;

        // Manejo del cooldown
        if (isInCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isInCooldown = false;
                agent.isStopped = false;
                currentAttackType= AttackType.None;
                bossDebilElemento = PlayerController.Elemento.None;
                invencibility=false;
                attackBuff=false;
                //animator.speed =1f;
                //cooldownActual = baseCooldown * 1f;
            }
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isApproaching && distanceToPlayer <= attackRangeClose)
        {
            isApproaching = false;
        }

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            ChasePlayer();
        }
        else
        {
            isChasing = false;
            Patrol();
        }

        if (isChasing && !isInCooldown && !isApproaching)
        {
            if (distanceToPlayer <= attackRangeMedium && distanceToPlayer > attackRangeClose)
            {
                AttackMediumRange();
            }
            else if (distanceToPlayer <= attackRangeClose)
            {
                AttackCloseRange();
            }
        }
    }

    private void StartCooldown()
    {
        isInCooldown = true;
        cooldownTimer = cooldownTiempo;
        agent.isStopped = true;
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
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        gameManager.paredBoss1.SetActive(true);
        gameManager.paredBoss2.SetActive(true);
    }

    public void TakeDamage(int pega)
    {
        int vidaVulnerable = fragmentosJugador * 25;
        int vidaMinimaPermitida = PVMax - vidaVulnerable;
        if(playerController.playerAtaqueElemento== PlayerController.Elemento.None)
        {
            playerController.manaActualPlayer=playerController.manaActualPlayer+25;
            if(playerController.manaActualPlayer>=100)
            {
                playerController.manaActualPlayer=100;
            }
        }

        if(currentAttackType == AttackType.None)
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
                if (gameManager.ElectricityEffect==true)
                {
                    bossDebilElemento = PlayerController.Elemento.Earth;
                    //animator.speed =1.5f;
                    //cooldownActual = baseCooldown * 0.5f;
                    PerformAttack(AttackType.Electricity);
                    StartCooldown();
                }
                else
                {
                    bossDebilElemento = PlayerController.Elemento.Earth;
                    PerformAttack(AttackType.Electricity);
                    StartCooldown();
                }
                break;
            case 1:
                if (gameManager.EarthEffect==true)
                {
                    bossDebilElemento = PlayerController.Elemento.Fire;
                    invencibility = true;
                    PerformAttack(AttackType.Earth);
                    StartCooldown();
                }
                else
                {
                    bossDebilElemento = PlayerController.Elemento.Fire;
                    PerformAttack(AttackType.Earth);
                    StartCooldown();
                }
                break;
            case 2:
                isApproaching = true;
                agent.SetDestination(player.position);
                break;
        }
    }

    private void AttackCloseRange()
    {
        int attackChoice = Random.Range(0, 2);
        switch (attackChoice)
        {
            case 0:
                if (gameManager.FireEffect==true)
                {
                    bossDebilElemento = PlayerController.Elemento.Water;
                    attackBuff=true;
                    PerformAttack(AttackType.Fire);
                    StartCooldown();
                }
                else
                {
                    bossDebilElemento = PlayerController.Elemento.Water;
                    PerformAttack(AttackType.Fire);
                    StartCooldown();
                }
                break;
            case 1:
                if (gameManager.WaterEffect==true)
                {
                    bossDebilElemento = PlayerController.Elemento.Electricity;
                    PerformAttack(AttackType.Water);
                    PVActual = Mathf.Min(PVActual + 15, PVMax);
                    StartCooldown();
                }
                else
                {
                    bossDebilElemento = PlayerController.Elemento.Electricity;
                    PerformAttack(AttackType.Water);
                    StartCooldown();
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
                Debug.Log("Boss realiza un ataque de Fuego!");
                break;
            case AttackType.Water:
                Debug.Log("Boss realiza un ataque de Agua!");
                break;
            case AttackType.Electricity:
                Debug.Log("Boss realiza un ataque de Electricidad!");
                break;
            case AttackType.Earth:
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
        muerto=true;
        //animator.SetTrigger("Muerte");
        //animator.speed = 0f;
        GameManager.Instance.BossDerrotado();
        gameManager.paredBoss1.SetActive(false);
        gameManager.paredBoss2.SetActive(false);
        
    }
}

