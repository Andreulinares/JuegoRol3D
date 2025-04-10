using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDetection : MonoBehaviour
{

    public bool enemigoDetectado = false;
    public bool enemigoEstaEnAreaInfluencia = false;
    public bool jugadorDetectado = false;
    // Start is called before the first frame update
    
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Melee")){
            if(gameObject.name == "DetectarMelee"){
                enemigoDetectado = true;
                Debug.Log("Melee detectado");
            }else if(gameObject.name == "AreaInfluencia"){
                enemigoEstaEnAreaInfluencia = true;
                Debug.Log("El melee esta en la area de influencia");
            }
        }else if (other.CompareTag("Player")){
            if(gameObject.name == "DetectarJugador"){
                jugadorDetectado = true;
                Debug.Log("Player detectado");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Melee")){
            if(gameObject.name == "DetectarMelee"){
                enemigoDetectado = false;
                Debug.Log("No detecto ningun enemigo melee");
            }else if(gameObject.name == "AreaInfluencia"){
                enemigoEstaEnAreaInfluencia = false;
                Debug.Log("El melee ya no esta en el area de influencia");
            }
        }else if (other.CompareTag("Player")){
            if(gameObject.name == "DetectarJugador"){
                jugadorDetectado = false;
                Debug.Log("No detecto ningun jugador");
            }
        }
    }
}
