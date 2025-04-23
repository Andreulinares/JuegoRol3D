using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public bool jugadorDetectado = false;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
                jugadorDetectado = true;
                Debug.Log("Jugador detectado");
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
                jugadorDetectado = false;
        }
    }
}
