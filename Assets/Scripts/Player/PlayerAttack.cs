using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int daño = 10;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) // (click izquierdo)
        {
            // Inicia la animación de ataque
            animator.SetBool("isAttack", true);
            Golpear();
        }
    }

    void Golpear(){
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("Enemy")) // Asegúrate de que el enemigo tenga el tag "Enemigo"
            {
                ElementalBehaviour enemigo = hit.collider.GetComponent<ElementalBehaviour>();
                if (enemigo != null)
                {
                    enemigo.TakeDamage(daño);
                    Debug.Log("Golpe al enemigo!");
                }
            }
        }
    }
}
