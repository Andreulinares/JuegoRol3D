using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public float attackRangeClose = 2f;
    public float attackRangeMedium = 5f;
    public float detectionRange = 10f;
    public int PVMax = 100;
    public int PVActual;
    public float AD = 5f;
    public float velocidad = 5f;
    public int fragmentosJugador = 0;
    public bool invencibility = false;

    public bool WaterEffect = true;
    public bool FireEffect = true;
    public bool EarthEffect = true;
    public bool ElectricityEffect = true;
    
    public enum AttackType { Fire, Water, Electricity, Earth, None }
    private AttackType currentAttackType = AttackType.None;

    private float distanceToPlayer;
    private bool isChasing = false;

    private void Start()
    {
        PVActual=PVMax;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

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

        if (isChasing)
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


    public void ActualizarFragmentos(int cantidad)
    {
        fragmentosJugador = Mathf.Clamp(cantidad, 0, 4); // máximo 4 fragmentos
    }
    private void Patrol()
    {
        // Lógica de patrullaje (puedes agregar puntos de patrullaje si es necesario)
        agent.SetDestination(transform.position + transform.forward);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void IntentarHacerDanio(int pega)
    {
        int vidaVulnerable = fragmentosJugador * 25;
        int vidaMinimaPermitida = PVMax - vidaVulnerable;

        // Solo permite bajar la vida hasta el mínimo permitido
        int nuevaVida = Mathf.Max(PVActual - pega, vidaMinimaPermitida);

        int pegaReal = PVActual - nuevaVida;
        PVActual = nuevaVida;

        Debug.Log("Se hizo " + pegaReal + " de daño. Vida actual: " + PVActual);
    }

    private void AttackMediumRange()
    {
        int attackChoice = Random.Range(0, 3); // 0 = primer ataque, 1 = segundo ataque, 2 = acercarse
        switch (attackChoice)
        {
            case 0:
                // Realizar el primer ataque a media distancia
                if (ElectricityEffect==true){
                    //Poner corazon Amarillo, hacer debil a la roca
                    velocidad = velocidad +3;
                    //Activar animacion con Electricidad
                    PerformAttack(AttackType.Electricity);
                }
                else{
                    //Poner corazon Amarillo, hacer debil a la roca
                    //Activar animacion sin Electricidad
                    PerformAttack(AttackType.Electricity);
                }
                
                break;
            case 1:
                // Realizar el segundo ataque a media distancia
                if(EarthEffect==true){
                    //Poner corazon marrón, hacer debil al fuego
                    invencibility = true;
                    //Activar animacion con Roca
                    PerformAttack(AttackType.Earth);
                }
                
                else{
                    //Poner corazon marrón, hacer debil al fuego
                    //Activar animacion sin Roca
                    PerformAttack(AttackType.Earth);
                }
                break;
            case 2:
                // Acercarse al jugador
                agent.SetDestination(player.position);
                break;
        }
    }

    private void AttackCloseRange()
    {
        int attackChoice = Random.Range(0, 2); // 0 = primer ataque, 1 = segundo ataque
        switch (attackChoice)
        {
            case 0:
                // Realizar el primer ataque a corta distancia
                if(FireEffect==true){
                    //Poner corazon rojo, hacer debil al agua
                    AD = AD + 3f;
                    //Activar animacion con fuego
                    PerformAttack(AttackType.Fire);
                }
                else{
                    //Poner corazon rojo, hacer debil al agua
                    //Activar animacion sin fuego
                    PerformAttack(AttackType.Fire);
                }
                break;
            case 1:
                // Realizar el segundo ataque a corta distancia
                if(WaterEffect==true){
                    //Poner corazon azul, hacer debil a la electricidad
                    //Activar aniamcion con agua
                    PerformAttack(AttackType.Water);
                    if(PVActual<=85){
                        PVActual=PVActual+15;
                    }
                    else if(PVActual>=86){
                        PVActual=100;
                    }
                }
                else{
                    //Poner corazon azul, hacer debil a la electricidad
                    //Activar aniamcion sin agua
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
                Debug.Log("Boss realiza un ataque de Fuego!");
                // Implementa el ataque de Fuego aquí
                break;
            case AttackType.Water:
                Debug.Log("Boss realiza un ataque de Agua!");
                // Implementa el ataque de Agua aquí
                break;
            case AttackType.Electricity:
                Debug.Log("Boss realiza un ataque de Electricidad!");
                // Implementa el ataque de Electricidad aquí
                break;
            case AttackType.Earth:
                Debug.Log("Boss realiza un ataque de Tierra!");
                // Implementa el ataque de Tierra aquí
                break;
            case AttackType.None:
                break;
        }
    }
}
