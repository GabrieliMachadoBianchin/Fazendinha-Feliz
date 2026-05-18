using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 720f;
    public float gravity = -20f;

    [Header("Interaction")]
    public float interactRange = 2.5f;
    public LayerMask interactLayer;

    [Header("Animation")]
    public Animator animator;

    private CharacterController cc;
    private Vector3 velocity;
    private Camera mainCam;
    private FarmPlot nearestPlot;
    private NPCTrader nearestNPC;
    private bool isInteracting;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        mainCam = Camera.main;
    }

    void Update()
    {
        if (isInteracting) return;

        HandleMovement();
        HandleInteraction();
        DetectNearbyObjects();
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;

        // Movimento relativo à câmera
        if (mainCam != null && input.magnitude > 0.1f)
        {
            Vector3 camForward = mainCam.transform.forward;
            Vector3 camRight = mainCam.transform.right;
            camForward.y = 0; camRight.y = 0;
            camForward.Normalize(); camRight.Normalize();

            Vector3 moveDir = (camRight * h + camForward * v).normalized;
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            cc.Move(moveDir * moveSpeed * Time.deltaTime);
        }

        // Gravidade
        if (cc.isGrounded) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        // Animação
        animator?.SetFloat("Speed", input.magnitude);
    }

    void HandleInteraction()
    {
        if (!Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.F)) return;

        // NPC
        if (nearestNPC != null)
        {
            nearestNPC.OpenShop();
            return;
        }

        // Plantar ou Colher
        if (nearestPlot != null)
        {
            if (nearestPlot.State == PlotState.Ready)
            {
                nearestPlot.Harvest();
                animator?.SetTrigger("Harvest");
            }
            else if (nearestPlot.State == PlotState.Empty)
            {
                UIManager.Instance?.OpenPlantingMenu(nearestPlot);
            }
        }
    }

    void DetectNearbyObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactLayer);
        nearestPlot = null;
        nearestNPC = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            float d = Vector3.Distance(transform.position, hit.transform.position);
            var plot = hit.GetComponent<FarmPlot>();
            if (plot != null && d < minDist)
            {
                minDist = d;
                nearestPlot = plot;
            }
            var npc = hit.GetComponent<NPCTrader>();
            if (npc != null)
            {
                nearestNPC = npc;
            }
        }

        UIManager.Instance?.UpdateInteractionHint(nearestPlot, nearestNPC);
    }

    public void SetInteracting(bool val) => isInteracting = val;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
