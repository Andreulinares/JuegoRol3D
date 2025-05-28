using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform jugador;
    public float altura = 20f;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void LateUpdate()
    {
        if (jugador != null)
        {
            transform.position = new Vector3(jugador.position.x, jugador.position.y + altura, jugador.position.z);
        }
        else
        {
            jugador = GameObject.FindGameObjectWithTag("Player")?.transform; 
        }
    }
}
