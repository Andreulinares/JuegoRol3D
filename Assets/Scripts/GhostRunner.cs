using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GhostRunner : MonoBehaviour
{
    private Animator enemigoAnimator;
    public GameObject elemental;
    public Transform[] patrolPoints;
    private NavMeshAgent agent;
    private int currentPoint;

    public float waitTime = 5f;
    private float waitTimer;
    // Start is called before the first frame update
    void Start()
    {
        enemigoAnimator = elemental.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        GetComponent<MeshRenderer>().enabled = false;
        //agent.stoppingDistance = 0.2f;
        if (patrolPoints.Length > 0)
        {
            currentPoint = 0;
            GoToNextPoint();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!agent.pathPending && agent.remainingDistance < 0.5)
        {
            /*if (enemigoAnimator != null)
            {
                enemigoAnimator.SetBool("isWalking", false);
            }
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                GoToNextPoint();
                waitTimer = 0;
            }*/
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        int nextPoint;
        do
        {
            nextPoint = Random.Range(0, patrolPoints.Length);
        } while (nextPoint == currentPoint && patrolPoints.Length > 1);

        currentPoint = nextPoint;
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    public void Detener(){
        agent.ResetPath();
    }
}
