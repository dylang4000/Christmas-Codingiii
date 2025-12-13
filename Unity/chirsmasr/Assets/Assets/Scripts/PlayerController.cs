using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f; // Adjust in the Inspector
    private Rigidbody rb;
    private Camera mainCamera;
    private bool jumpRequested = false;
    private bool isGrounded; // To check if the ball is touching the ground

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check for jump input in Update to ensure it's captured
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate() // Use FixedUpdate for physics operations
    {
        // Get input from keyboard/joystick
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction relative to the camera's view
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraForward.y = 0; // Keep movement along the ground plane
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = (cameraForward * verticalInput + cameraRight * horizontalInput) * moveSpeed;

        // Apply force to the Rigidbody
        rb.AddForce(movement);

        // Apply jump force in FixedUpdate if requested
        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Use Impulse for instant jump force
            jumpRequested = false;
            isGrounded = false; // The ball is no longer grounded
        }
    }

    // Check for ground collisions
    void OnCollisionEnter(Collision collision)
    {
        // You might want to check if collision.gameObject.CompareTag("Ground")
        isGrounded = true;
    }
}
