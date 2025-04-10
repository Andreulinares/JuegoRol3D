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
            }else if(gameObject.name == "AreaInfluencia"){
                enemigoEstaEnAreaInfluencia = true;
            }
        }else if (other.CompareTag("Player")){
            if(gameObject.name == "DetectarJugador"){
                jugadorDetectado = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Melee")){
            if(gameObject.name == "DetectarMelee"){
                enemigoDetectado = false;
            }else if(gameObject.name == "AreaInfluencia"){
                enemigoEstaEnAreaInfluencia = false;
            }
        }else if (other.CompareTag("Player")){
            if(gameObject.name == "DetectarJugador"){
                jugadorDetectado = false;
            }
        }
    }
}
