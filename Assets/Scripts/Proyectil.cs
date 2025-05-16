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
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(daño);
            Debug.Log("Impactó al jugador");
            Destroy(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
}
