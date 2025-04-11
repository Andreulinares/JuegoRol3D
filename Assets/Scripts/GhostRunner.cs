using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GhostRunner : MonoBehaviour
{
    public float radioPatrullaje = 8f;
    public float tiempoEspera = 3f;
    private NavMeshAgent agent;
    private Vector3 puntoPatrullaje;
    private float temporizadorEspera;
    private bool isWaiting;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaiting)
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
        }
    }
    void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radioPatrullaje;
        randomDirection += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, radioPatrullaje, 1))
        {
            puntoPatrullaje = hit.position;
            agent.SetDestination(puntoPatrullaje);
        }
    }
}
