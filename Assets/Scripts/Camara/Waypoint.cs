using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private Transform target;
    public Vector3 offset = new Vector3(0, 2, 0);

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);

            if (screenPos.z > 0)
                transform.position = screenPos;
            else
                transform.position = new Vector3(-1000, -1000, 0); 
        }
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
