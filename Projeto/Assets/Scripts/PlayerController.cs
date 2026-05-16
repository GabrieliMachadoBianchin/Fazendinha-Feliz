using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;

    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0f, v);

        // Move no espaço do mundo
        controller.Move(move * speed * Time.deltaTime);

        // Rotaciona
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // Gravidade
        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        // Animator
        animator.SetFloat("Hor", h);
        animator.SetFloat("Vert", v);
        animator.SetBool("IsJump", !controller.isGrounded);
    }
}