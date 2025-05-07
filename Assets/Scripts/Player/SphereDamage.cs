using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SphereDamage : MonoBehaviour
{
    public int damage = 10; // Daño que hace la esfera
    public float lifetime = 2f; // Tiempo que la esfera permanecerá antes de desaparecer
    public EnemigoMelee enemigoMelee;
    public ElementalBehaviour elementalBehaviour;
    public BossAI bossAI;
    public PlayerController playerController;

    private void Start()
    {
        // Destruir la esfera después de un tiempo
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entra en contacto tiene el tag "Enemy"
        if (other.CompareTag("Boss"))
        {
                bossAI.TakeDamage(damage);

            // Después de hacer el daño, destruimos la esfera
            Destroy(gameObject);
        }
        else if (other.CompareTag("Melee"))
        {
                enemigoMelee.TakeDamage(damage);
            // Después de hacer el daño, destruimos la esfera
            Destroy(gameObject);
        }
        else if (other.CompareTag("Elemental"))
        {
                elementalBehaviour.TakeDamage(damage);
            // Después de hacer el daño, destruimos la esfera
            Destroy(gameObject);
        }
    }
}