using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public float speed = 4f;
    public float jumpHeight = 1f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;

    private Vector3 velocity;
    private bool isGrounded;

    //public Transform cameraTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // Obtener entrada del jugador
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Obtener dirección basada en la cámara
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = (cameraForward * moveVertical + cameraRight * moveHorizontal);

        if (movement.magnitude > 1f)
        {
            movement.Normalize();  
        }

        // Mover al personaje
        controller.Move(movement * speed * Time.deltaTime);

        if (movement.magnitude > 0.1f) // Solo rota si se está moviendo
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Manejar salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
        
}
