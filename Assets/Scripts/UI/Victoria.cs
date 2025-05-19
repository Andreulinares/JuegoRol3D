using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victoria : MonoBehaviour
{
    public ReiniciarJuego reiniciarJuego;
    public GameObject pantallaVictoria;
    public void botonPulsado()
    {
        reiniciarJuego.reiniciarJuego();
        pantallaVictoria.SetActive(false);
    }
}
