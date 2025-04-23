using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDetection : MonoBehaviour
{

    public enum TipoDetector { DetectarMelee, AreaInfluencia, DetectarJugador }
    public TipoDetector tipoDetector;

    private ElementalBehaviour elemental; 

    void Start()
    {
        elemental = GetComponentInParent<ElementalBehaviour>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee") && tipoDetector == TipoDetector.DetectarMelee)
        {
            elemental.EnemigoDetectado(true);
            Debug.Log("Melee detectado");
        }
        else if (other.CompareTag("Player") && tipoDetector == TipoDetector.DetectarJugador)
        {
            elemental.JugadorDetectado(true);
            Debug.Log("Jugador detectado");
        }
        else if (other.CompareTag("Melee") && tipoDetector == TipoDetector.AreaInfluencia)
        {
            elemental.EnemigoEnArea(true);
            other.GetComponent<EnemigoMelee>().NotificarEstadoArea(true);
            Debug.Log("El melee está en la área de influencia");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Melee") && tipoDetector == TipoDetector.DetectarMelee)
        {
            elemental.EnemigoDetectado(false);
            Debug.Log("No detecto ningún enemigo melee");
        }
        else if (other.CompareTag("Player") && tipoDetector == TipoDetector.DetectarJugador)
        {
            elemental.JugadorDetectado(false);
            Debug.Log("No detecto ningún jugador");
        }
        else if (other.CompareTag("Melee") && tipoDetector == TipoDetector.AreaInfluencia)
        {
            elemental.EnemigoEnArea(false);
            other.GetComponent<EnemigoMelee>().NotificarEstadoArea(false);
            Debug.Log("El melee ya no está en la área de influencia");
        }
    }
}
