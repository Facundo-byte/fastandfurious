using UnityEngine;

public class CameraFollowCar : MonoBehaviour
{
    [Header("Target")]
    public Transform carTarget; // el auto

    [Header("Suavizado")]
    public float moveSmoothness = 5f;
    public float rotSmoothness = 5f;

    [Header("Suavizado")]
    public Vector3 moveOffset;
    public Vector3 rotOffset;

    public float maxDistance = 10f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 targetPos = carTarget.TransformPoint(moveOffset);

        // Dirección desde el auto
        Vector3 dir = (targetPos - carTarget.position).normalized;

        // Limitar directamente
        targetPos = carTarget.position + dir * Mathf.Min(Vector3.Distance(targetPos, carTarget.position), maxDistance);

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
         
    }

    void HandleRotation()
    {
        var direction = carTarget.position - transform.position;
        var rotation = new Quaternion();

        rotation = Quaternion.LookRotation(direction + rotOffset, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSmoothness * Time.deltaTime);
    }
}