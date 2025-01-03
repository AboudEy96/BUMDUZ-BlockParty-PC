using System;
using UnityEngine;

public abstract class Player : MonoBehaviour
{ 
    // MOVE FORCES
    private float moveSpeed = 5.0f;
    private float rotationSpeed = 10.0f;
    
    // JUMP FORCES
    private float jumpForce = 2.0f;
    private float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Camera Settings")]
    public Transform cameraTransform;

    private PlayerController characterController;

    private void Awake()
    {
        characterController = new PlayerController(GetComponent<CharacterController>());
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (characterController.IsGrounded(groundCheck.position, groundDistance, groundMask))
        {
            characterController.ResetVerticalVelocity();

            if (Input.GetButtonDown("Jump"))
            {
                characterController.Jump(jumpForce, gravity);
            }
        }

        Vector3 moveDirection = characterController.CalculateMoveDirection(cameraTransform, Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        characterController.RotateTowards(moveDirection, rotationSpeed);
        characterController.Move(moveDirection, moveSpeed);
    }
}

