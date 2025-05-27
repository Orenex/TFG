using UnityEngine;

public class Movimiento_Libre : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private Animator animator;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        velocity.y = -2f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Obtenemos la direcci�n de la c�mara
        Transform cam = Camera.main.transform;

        // Direcci�n plana (sin inclinaci�n vertical)
        Vector3 forward = cam.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cam.right;
        right.y = 0;
        right.Normalize();

        // Calculamos la direcci�n en funci�n de la c�mara
        Vector3 move = forward * vertical + right * horizontal;

        if (move != Vector3.zero)
        {
            transform.forward = move;
            animator.SetBool("IsWalking",true);
            
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        controller.Move(move.normalized * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
