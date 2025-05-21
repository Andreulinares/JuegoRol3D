using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEditor.Animations;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public int daño = 10;
    private Collider golpe;
    public BossAI bossAI;
    // Start is called before the first frame update

    void Start()
    {
        golpe = GetComponent<Collider>();
        golpe.enabled = false; 
    }

    public void ActivarColliderGolpe()
    {
        golpe.enabled = true; 
        Invoke("DesactivarColliderGolpe", 5f); 
    }

    private void DesactivarColliderGolpe()
    {
        golpe.enabled = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bossAI.attackBuff == true)
        {
            if (other.CompareTag("Player"))   
        {
            other.GetComponent<PlayerController>().TakeDamage(daño + 5);
            Debug.Log("Golpe impacto a player!");

            other.GetComponent<ArqueroController>().TakeDamage(daño + 5);

            Debug.Log("Golpe impactó a arquero!");
            bossAI.isAttacking = false;
        }
        }
        else
        {
            if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(daño);
            Debug.Log("Golpe impacto a player!");

            other.GetComponent<ArqueroController>().TakeDamage(daño);
            Debug.Log("Golpe impactó a arquero!");
            bossAI.isAttacking = false;
        }
        }
        
    }
}
