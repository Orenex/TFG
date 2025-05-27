using UnityEngine;

// Controla el movimiento libre del jugador en tercera persona
public class Movimiento_Libre : MonoBehaviour
{
    public float speed = 5f;                 // Velocidad de movimiento
    public float gravity = -9.81f;           // Gravedad aplicada al personaje

    private CharacterController controller;  // Componente que maneja colisiones y movimiento
    private Vector3 velocity;                // Velocidad vertical (gravedad)
    private Vector3 moveDirection;           // Direcci�n de movimiento
    private Animator animator;               // Controlador de animaciones

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Reinicia el componente vertical de la velocidad
        velocity.y = -2f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Obtiene direcci�n de la c�mara actual
        Transform cam = Camera.main.transform;

        // Obtiene direcci�n horizontal (plana) de la c�mara
        Vector3 forward = cam.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cam.right;
        right.y = 0;
        right.Normalize();

        // Calcula direcci�n final seg�n entrada y orientaci�n de c�mara
        Vector3 move = forward * vertical + right * horizontal;

        if (move != Vector3.zero)
        {
            transform.forward = move;               // Gira el personaje
            animator.SetBool("IsWalking", true);    // Activa animaci�n de caminar
        }
        else
        {
            animator.SetBool("IsWalking", false);   // Detiene animaci�n si no hay movimiento
        }

        // Mueve al personaje en direcci�n normalizada
        controller.Move(move.normalized * speed * Time.deltaTime);

        // Aplica gravedad manualmente
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
