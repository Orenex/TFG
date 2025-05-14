using UnityEngine;

public class Movimiento_Libre : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 moveDirection;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        velocity.y = -2f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0, vertical);

        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
        }

        controller.Move(moveDirection.normalized * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
