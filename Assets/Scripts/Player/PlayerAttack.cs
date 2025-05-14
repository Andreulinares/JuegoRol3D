using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int daño = 10;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("enemy")) // Verifica que el objeto golpeado es un enemigo
    {
        other.GetComponent<EnemigoMelee>().TakeDamage(daño);
        Debug.Log("Golpe impactó al enemigo!");
    }
}
}
