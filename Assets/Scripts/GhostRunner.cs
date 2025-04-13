using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GhostRunner : MonoBehaviour
{
    /*public float radioPatrullaje = 8f;
    public float tiempoEspera = 3f;
    private NavMeshAgent agent;
    private Vector3 puntoPatrullaje;
    private float temporizadorEspera;
    private bool isWaiting;*/
    public Transform CentroPatrullaje;
    public float radio = 10f;
    public float esperaEntreMovimientos = 2f;
    private float temporizador = 0f;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetComponent<MeshRenderer>().enabled = false;

        MoverANuevoPunto();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (isWaiting)
        {
            temporizadorEspera -= Time.deltaTime;
            if (temporizadorEspera <= 0)
            {
                isWaiting = false;
                SetNewPatrolPoint();
            }
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 1)
        {
            isWaiting = true;
            temporizadorEspera = tiempoEspera;
        }*/

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            temporizador += Time.deltaTime;
            if (temporizador >= esperaEntreMovimientos)
            {
                MoverANuevoPunto();
                temporizador = 0f;
            }
        }
    }

    void MoverANuevoPunto()
    {
        Vector3 puntoRandom;
        if (PuntoAleatorio(CentroPatrullaje.position, radio, out puntoRandom))
        {
            agent.SetDestination(puntoRandom);
        }
    }

    bool PuntoAleatorio(Vector3 centro, float radio, out Vector3 resultado)
    {
        for (int i = 0; i < 30; i++) // intentar varias veces si hace falta
        {
            Vector3 randomPos = centro + Random.insideUnitSphere * radio;
            randomPos.y = centro.y; // mantener altura
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
            {
                resultado = hit.position;
                return true;
            }
        }
        resultado = Vector3.zero;
        return false;
    }
    /*void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radioPatrullaje;
        randomDirection += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, radioPatrullaje, 1))
        {
            puntoPatrullaje = hit.position;
            agent.SetDestination(puntoPatrullaje);
        }
    }*/
}
