using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDamage : MonoBehaviour
{
    public int damage = 10; // Daño que hace la esfera
    public float lifetime = 2f; // Tiempo que la esfera permanecerá antes de desaparecer

    private void Start()
    {
        // Destruir la esfera después de un tiempo
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entra en contacto tiene el tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Obtener el componente Enemy del objeto y aplicar el daño
            BossAI enemy = other.GetComponent<BossAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, BossAI.AttackType.Electricity);
            }

            // Después de hacer el daño, destruimos la esfera
            Destroy(gameObject);
        }
    }
}
