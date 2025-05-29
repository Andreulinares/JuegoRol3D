using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    // Start is called before the first frame update
    private Diario1 itemActual;

    public void SetDiario(Diario1 item)
    {
        itemActual = item;
    }

    // Este método lo llamará el evento en la animación
    public void EjecutarRecogida()
    {
        if (itemActual != null)
        {
            itemActual.EjecutarRecogida();
            itemActual = null;
        }
    }
}
