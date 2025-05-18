using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public int daño = 20;
    private Collider golpe;
    // Start is called before the first frame update
    void Start()
    {
        golpe = GetComponent<Collider>();
        golpe.enabled = false; 
    }

    public void ActivarColliderGolpe()
    {
        golpe.enabled = true; 
        Invoke("DesactivarColliderGolpe", 0.5f); 
    }

    private void DesactivarColliderGolpe()
    {
        golpe.enabled = false; 
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
            Debug.Log("Golpe impactó a player!");
        }else if(other.CompareTag("Player2")){
            other.GetComponent<ArqueroController>().TakeDamage(daño);
            Debug.Log("Golpe impactó a player2!");
        }*/

        if (other.CompareTag("Player")){
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null){
                player.TakeDamage(daño);
                Debug.Log("Golpe impactó a player1!");
            }

            ArqueroController arquero = other.GetComponent<ArqueroController>();
            if (arquero != null){
                arquero.TakeDamage(daño);
                Debug.Log("Golpe impactó a arquero!");
            }
        }
    }
}
