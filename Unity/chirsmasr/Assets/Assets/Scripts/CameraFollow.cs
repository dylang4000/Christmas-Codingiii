using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The ball
    public Transform pivot; // The CameraPivot object (assign in Inspector)
    public Vector3 offset = new Vector3(0f, 5f, -10f); 
    public float smoothSpeed = 0.125f;
    public float mouseSensitivity = 3f; // Adjust camera rotation speed

    private float rotationX = 0f;
    private float rotationY = 0f;

    // Define the vertical limits
    private const float MIN_VERTICAL_ANGLE = -80.0f; // Maximum angle down (e.g., looking almost straight down)
    private const float MAX_VERTICAL_ANGLE = 0.0f;  // Maximum angle up (e.g., restricted to horizon level)

    void Start()
    {
        // Lock and hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (target == null || pivot == null)
            return;

        // Get mouse input for rotation
        rotationY += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity; // Invert Y for natural feel

        // Clamp the vertical angle:
        // This line ensures rotationX never goes above 0.0f (no looking up past the horizon).
        rotationX = Mathf.Clamp(rotationX, MIN_VERTICAL_ANGLE, MAX_VERTICAL_ANGLE);

        // Apply rotation to the pivot
        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);

        // Move pivot to ball's position
        pivot.position = target.position; 
        
        // Smoothly rotate the pivot to the target rotation
        pivot.rotation = Quaternion.Lerp(pivot.rotation, targetRotation, smoothSpeed * 2);

        // Position the camera relative to the rotated pivot and offset
        Vector3 desiredPosition = pivot.position + pivot.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Ensure the camera always looks at the pivot point
        transform.LookAt(pivot.position);
    }
}
