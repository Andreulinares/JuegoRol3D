using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Boss_patrol : MonoBehaviour
{
    public GameObject ghost; 
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ghost != null)
        {
            agent.SetDestination(ghost.transform.position);
        }
    }

    public void ActivarPatrullaje()
    {
        enabled = true;
    }

    public void DesactivarPatrullaje(){
        enabled = false;
        agent.ResetPath();
    }
}
