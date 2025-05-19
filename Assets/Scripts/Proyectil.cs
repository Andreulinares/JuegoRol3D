using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float tiempoDeVida = 3f;
    public int daño = 20;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(daño);
                Debug.Log("Proyectil impactó a player1!");
                Destroy(gameObject);
            }

            ArqueroController arquero = collision.gameObject.GetComponent<ArqueroController>();
            if (arquero != null)
            {
                arquero.TakeDamage(daño);
                Debug.Log("Proyectil impactó a arquero!");
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject, tiempoDeVida);
        }
    }
}