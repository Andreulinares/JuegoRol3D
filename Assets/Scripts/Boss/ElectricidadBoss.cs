using System.Collections;
using System.Collections.Generic;
using StarterAssets;
//using UnityEditor.Animations;
using UnityEngine;

public class ElectricidadBoss : MonoBehaviour
{
    public BossAI bossAI;
    public int daño = 10;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(daño);
            Debug.Log("Golpe electricidad impacto a player!");

            other.GetComponent<ArqueroController>().TakeDamage(daño);
            Debug.Log("Golpe electricidad impactó a arquero!");
            bossAI.isAttacking = false;
        }
    }
}
