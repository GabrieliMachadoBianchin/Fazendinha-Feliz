using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float smoothSpeed = 5f;

    private Vector3 offset;

    void Start()
    {
        // Guarda a distância inicial da câmera
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Offset acompanha a rotação do personagem
        Vector3 rotatedOffset = player.rotation * offset;

        // Posição desejada
        Vector3 desiredPosition = player.position + rotatedOffset;

        // Movimento suave
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // Rotação suave acompanhando o personagem
        Quaternion desiredRotation =
            Quaternion.LookRotation(player.forward);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            desiredRotation,
            smoothSpeed * Time.deltaTime
        );
    }
}
/*using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public Vector3 offset = new Vector3(0, 2, -4);

    void LateUpdate()
    {
        transform.position = player.position + offset;

        transform.LookAt(player);
    }
}*/