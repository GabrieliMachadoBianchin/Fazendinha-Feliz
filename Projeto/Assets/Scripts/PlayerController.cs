using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 8f;
    public float gravity = -20f;

    [Header("Rotação")]
    public float rotationSpeed = 360f;

    private CharacterController controller;
    private Vector3 velocity;

    [Header("Animator")]
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
    }

    void Update()
    {
        // INPUT
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // DIREÇÃO
        Vector3 move = new Vector3(h, 0f, v);

        // Corrige diagonal
        move = Vector3.ClampMagnitude(move, 1f);

        // MOVIMENTO
        controller.Move(move * speed * Time.deltaTime);

        // ROTAÇÃO SUAVE
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // GRAVIDADE
        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        // ANIMATOR
        if (animator != null)
        {
            animator.SetFloat("Hor", h);
            animator.SetFloat("Vert", v);
            animator.SetBool("IsJump", !controller.isGrounded);
        }
    }
}