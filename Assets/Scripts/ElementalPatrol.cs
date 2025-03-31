using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class ElementalPatrol : MonoBehaviour
{
    // Start is called before the first frame update
    public float radioPatrullaje = 10f;
    public float tiempoEspera = 3f;
    private NavMeshAgent agent;
    private Vector3 puntoPatrullaje;
    private float temporizadorEspera;
    private bool isWaiting;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaiting){
            temporizadorEspera -= Time.deltaTime;

            if (temporizadorEspera <= 0){
                isWaiting = false;
                SetNewPatrolPoint();
            }else{
                Debug.Log("El temporizador aun no ha finalizado");
            }
        }

        if (!agent.pathPending && agent.remainingDistance < 1){
            isWaiting = true;
            temporizadorEspera = tiempoEspera;
        }
    }

    void SetNewPatrolPoint(){
        
    }
}
