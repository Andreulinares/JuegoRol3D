using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuerreroMovimiento : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        /*float mouseX = Input.GetAxis("Mouse X") * 5f;
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + mouseX, 0f);*/
    }
}
