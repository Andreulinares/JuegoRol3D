using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArqueroAttack : MonoBehaviour
{

    public int daño = 10;
    public float tiempoVida = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo")) 
        {
            other.GetComponent<EnemigoMelee>().TakeDamage(daño);
            Debug.Log("¡Flecha impactó al enemigo!");
            Destroy(gameObject); 
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
