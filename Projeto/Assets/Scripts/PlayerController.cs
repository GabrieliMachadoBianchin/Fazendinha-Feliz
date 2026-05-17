using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float gravity = -20f;
    public float rotationSpeed = 10f;

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
/*
        Vector3 move = new Vector3(h, 0f, v);

        // Move no espaço do mundo
        controller.Move(move * speed * Time.deltaTime);
*/

        Vector3 move = transform.forward * v + transform.right * h;

        //move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * speed * Time.deltaTime);

        // Rotaciona
        /*
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }*/
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
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