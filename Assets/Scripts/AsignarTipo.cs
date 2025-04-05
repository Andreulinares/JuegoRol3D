using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsignarTipo : MonoBehaviour
{
    public enum TipoElemental{
        Fuego,
        Agua,
        Tierra,
        Electricidad
    }
    
    public void AsignarTipoElemental(EnemigoMelee enemigo){
        if (enemigo == null || enemigo.tieneTipo){
            return;
        }else{
            TipoElemental tipoAleatorio = (TipoElemental)Random.Range(0, System.Enum.GetValues(typeof(TipoElemental)).Length);
            enemigo.ObtenerTipoElemental(tipoAleatorio);
            Debug.Log("Tipo elemental asignado: " + tipoAleatorio);
        }
    }
}
