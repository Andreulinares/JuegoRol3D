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

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(daño);
            Debug.Log("Impactó al jugador");
            Destroy(gameObject);
        }else if(other.CompareTag("Player2")){
            other.GetComponent<ArqueroController>().TakeDamage(daño);
            Debug.Log("Impactó a player2!");
            Destroy(gameObject);
        }else{
            Destroy(gameObject);
        }*/

        if (other.CompareTag("Player")){
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null){
                player.TakeDamage(daño);
                Debug.Log("Proyectil impactó a player1!");
                Destroy(gameObject);
            }

            ArqueroController arquero = other.GetComponent<ArqueroController>();
            if (arquero != null){
                arquero.TakeDamage(daño);
                Debug.Log("Proyectil impactó a arquero!");
                Destroy(gameObject);
            }
        }else{
            Destroy(gameObject);
        }
    }
}
