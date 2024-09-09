using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -20f;  // Increased gravity for faster falling
    public float jumpHeight = 1.5f; // Reduced jump height for quicker, more responsive jumps

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform playerBody;
    public Transform playerCamera;
    public float mouseSensitivity = 100f;
    public float smoothTime = 0.1f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float xRotationVelocity = 0f;
    private float yRotationVelocity = 0f;

    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MovePlayer();
        MouseLook();
    }

    void MovePlayer()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensures a small downward force to keep player grounded
        }

        // Get input for movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move the player in the direction based on camera orientation
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calculates upward velocity based on jump height and gravity
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move player based on gravity
        controller.Move(velocity * Time.deltaTime);
    }

    void MouseLook()
    {
        // Get mouse input for look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Update rotation values
        yRotation += mouseX;
        xRotation -= mouseY;

        // Clamp vertical rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Smooth horizontal rotation
        float smoothedYRotation = Mathf.SmoothDampAngle(playerBody.eulerAngles.y, yRotation, ref yRotationVelocity, smoothTime);
        playerBody.eulerAngles = new Vector3(0, smoothedYRotation, 0);

        // Apply vertical rotation directly
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
