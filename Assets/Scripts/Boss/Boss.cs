using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public GameManager gameManager;
    public Transform player;
    public NavMeshAgent agent;
    public int attackRangeClose = 2;
    public int attackRangeMedium = 5;
    public int detectionRange = 10;
    public int PVMax = 100;
    public int PVActual;
    public int AD = 5;
    public int AS = 5;
    public int fragmentosJugador = 0;
    public bool invencibility = false;

    public enum AttackType { Fire, Water, Electricity, Earth, None }
    private AttackType currentAttackType = AttackType.None;
    private AttackType debilidadActual = AttackType.None;

    private float distanceToPlayer;
    private bool isChasing = false;
    private bool isInCooldown = false;
    private bool isApproaching = false;
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

        fragmentosJugador=gameManager.fragmentosRecolectados;

        // Manejo del cooldown
        if (isInCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isInCooldown = false;
                agent.isStopped = false;
                debilidadActual = AttackType.None;
                invencibility=false;
                AD=5;
                AS=5;
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

    private void Patrol()
    {
        agent.SetDestination(transform.position + transform.forward);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void TakeDamage(int pega, AttackType tipoAtaque)
    {
        int vidaVulnerable = fragmentosJugador * 25;
        int vidaMinimaPermitida = PVMax - vidaVulnerable;

        if (tipoAtaque == debilidadActual)
        {
            pega += 4;
            Debug.Log("¡Daño crítico! El ataque fue efectivo contra la debilidad del boss.");
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
                    debilidadActual = AttackType.Earth;
                    AS += 3;
                    PerformAttack(AttackType.Electricity);
                    StartCooldown();
                }
                else
                {
                    debilidadActual = AttackType.Earth;
                    PerformAttack(AttackType.Electricity);
                    StartCooldown();
                }
                break;
            case 1:
                if (gameManager.EarthEffect==true)
                {
                    debilidadActual = AttackType.Fire;
                    invencibility = true;
                    PerformAttack(AttackType.Earth);
                    StartCooldown();
                }
                else
                {
                    debilidadActual = AttackType.Fire;
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
                    debilidadActual = AttackType.Water;
                    AD += 3;
                    PerformAttack(AttackType.Fire);
                    StartCooldown();
                }
                else
                {
                    debilidadActual = AttackType.Water;
                    PerformAttack(AttackType.Fire);
                    StartCooldown();
                }
                break;
            case 1:
                if (gameManager.WaterEffect==true)
                {
                    debilidadActual = AttackType.Electricity;
                    PerformAttack(AttackType.Water);
                    PVActual = Mathf.Min(PVActual + 15, PVMax);
                    StartCooldown();
                }
                else
                {
                    debilidadActual = AttackType.Electricity;
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

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        //animator.SetTrigger("Muerte");
        //animator.speed = 0f;
        GameManager.Instance.CambiarEscena("FinalScene");
        GameManager.Instance.BossDerrotado();
    }
}

