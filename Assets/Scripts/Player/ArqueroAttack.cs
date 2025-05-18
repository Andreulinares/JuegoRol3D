using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArqueroAttack : MonoBehaviour
{

    public int daño = 10;
    public float tiempoVida = 10f;
    // Start is called before the first frame update
    void Start()
    {
        //Ignorar colisiones de personaje y arco para evitar problemas
        Collider flechaCollider = GetComponent<Collider>();
        Collider arcoCollider = GameObject.FindGameObjectWithTag("arco")?.GetComponent<Collider>();
        Collider personajeCollider = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Collider>();

        if (arcoCollider != null)
        {
            Physics.IgnoreCollision(flechaCollider, arcoCollider);
        }

            if (personajeCollider != null)
        {
            Physics.IgnoreCollision(flechaCollider, personajeCollider);
        }

        Destroy(gameObject, tiempoVida);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            other.GetComponent<EnemigoMelee>().TakeDamage(daño);
            Debug.Log("¡Flecha impactó al enemigo!");
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            transform.SetParent(other.transform);
            Debug.Log("¡Flecha impactó contra un objeto sólido!");
        }
        else 
        {
            Destroy(gameObject);
        }
    }*/
    
    private void OnCollisionEnter(Collision collision)
{
        if (collision.gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<EnemigoMelee>().TakeDamage(daño);
            Debug.Log("¡Flecha impactó al enemigo!");
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        { 
            Debug.Log("Flecha impactó en el jugador, no se clava.");
            return;
        }
        else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Evitar que la física afecte a la flecha
            }

            // Hacer que la flecha se convierta en hija del objeto colisionado
            transform.SetParent(collision.transform);

            // Opcional: Ajustar la rotación para que se quede clavada
            transform.position = collision.contacts[0].point;
        }
}
}
