using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MoveClick : MonoBehaviour
{

    public Camera mainCamera;

    public GameObject destination;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("Destination");
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetMouseButtonDown(0)){
            Move();
        }*/
    }

    /*void Move(){
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)){
            agent.SetDestination(hit.point);
        }
    }*/
}
