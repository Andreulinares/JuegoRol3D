using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPrueba : MonoBehaviour
{

    public float speed = 5f;
    public float rotationSpeed = 720f;
    private Animator animator;
    public CharacterController controller;

    private Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Movimiento
        if (moveDirection.magnitude > 0)
        {
            controller.Move(moveDirection * speed * Time.deltaTime);
            transform.forward = moveDirection;
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space)) // Barra espaciadora para saltar
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.F)) // La tecla "F" para atacar
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

        // Muerte
        if (Input.GetKeyDown(KeyCode.K)) // La tecla "K" para la animación de muerte
        {
            animator.SetBool("isDead", true);
        }
    }
    }
