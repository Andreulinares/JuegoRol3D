using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GhostMeleeRunner : MonoBehaviour
{
    public Transform CentroPatrullaje;
    public float radio = 10f;
    public float cambioDistancia = 1f;
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
        if (!agent.pathPending && agent.remainingDistance <= cambioDistancia)
        {
            MoverANuevoPunto();
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
        for (int i = 0; i < 20; i++) 
        {
            Vector3 randomPos = centro + Random.insideUnitSphere * radio;
            randomPos.y = centro.y; 
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
}
