using UnityEngine;

public class CameraFollowCar : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // el auto

    [Header("Distancia y altura")]
    public float distance = 6.0f;
    public float height = 2.5f;

    [Header("Suavizado")]
    public float positionSmoothSpeed = 5f;
    public float rotationSmoothSpeed = 5f;

    [Header("Rotación con mouse")]
    public float mouseSensitivity = 3f;
    public float maxLookAngle = 60f;
    public float minLookAngle = -20f;

    private float currentYaw = 0f;
    private float currentPitch = 10f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Input del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        currentYaw += mouseX;
        currentPitch = Mathf.Clamp(currentPitch, minLookAngle, maxLookAngle);

        // Rotación base del auto + input del jugador
        Quaternion rotation = Quaternion.Euler(currentPitch, target.eulerAngles.y + currentYaw, 0);

        // Posición deseada detrás del auto
        Vector3 desiredPosition = target.position 
                                - rotation * Vector3.forward * distance 
                                + Vector3.up * height;

        // Movimiento suave
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * positionSmoothSpeed);

        // Mirar al auto
        Vector3 lookTarget = target.position + Vector3.up * 1.5f;
        Quaternion lookRotation = Quaternion.LookRotation(lookTarget - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSmoothSpeed);
    }
}