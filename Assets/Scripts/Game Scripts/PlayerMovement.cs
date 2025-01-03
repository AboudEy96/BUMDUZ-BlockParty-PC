using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float rotationSpeed = 10f; 
    public float jumpHeight = 2f; 
    public float gravity = -9.81f; 
    public Transform cameraTransform; 
    public Transform groundCheck; 
    public float groundDistance = 0.4f; 
    public LayerMask groundMask; 

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // G CHECK
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        
        float horizontal = Input.GetAxisRaw("Horizontal"); // ad
        float vertical = Input.GetAxisRaw("Vertical"); // ws

        
        Vector3 moveDirection = cameraTransform.right * horizontal + cameraTransform.forward * vertical;
        moveDirection.y = 0; 

        if (moveDirection.magnitude > 0.1f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // move
        controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
