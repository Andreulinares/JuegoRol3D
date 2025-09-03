using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : MonoBehaviour
{
    public float duracionEscudo = 5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ActivarEscudo()
    {
        gameObject.SetActive(true);
    }

    public void DesactivarEscudo()
    {
        gameObject.SetActive(false);
    }

        private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Debug.Log("¡El enemigo ha chocado contra el escudo!");
            
            // Bloquear enemigo
            collision.transform.position -= collision.transform.forward * 0.1f; 
        }
    }
}
