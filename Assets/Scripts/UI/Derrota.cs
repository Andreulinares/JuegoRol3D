using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Derrota : MonoBehaviour
{
    public PlayerController playerController;
    public ArqueroController arqueroController;
    public GameObject pantallaMuerte;
    public void botonPulsado()
    {
        playerController.ReiniciarPersonaje();
        arqueroController.ReiniciarPersonaje();
        pantallaMuerte.SetActive(false);

    }
}
