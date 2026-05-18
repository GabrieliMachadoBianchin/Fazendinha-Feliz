using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 20, -8);

    [Header("Smoothing")]
    public float followSpeed = 8f;
    public float rotateSpeed = 60f;

    [Header("Zoom")]
    public float minZoom = 5f;
    public float maxZoom = 15f;
    public float zoomSpeed = 3f;

    private float currentZoomDist;
    private float currentYAngle = 45f;

    void Start()
    {
        currentZoomDist = offset.magnitude;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Rotação da câmera com Q/E
        if (Input.GetKey(KeyCode.Q)) currentYAngle -= rotateSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) currentYAngle += rotateSpeed * Time.deltaTime;

        // Zoom com scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoomDist = Mathf.Clamp(currentZoomDist - scroll * zoomSpeed, minZoom, maxZoom);

        // Calcula posição
        Quaternion rotation = Quaternion.Euler(40f, currentYAngle, 0);
        Vector3 desiredPos = target.position + rotation * new Vector3(0, 0, -currentZoomDist);

        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}
