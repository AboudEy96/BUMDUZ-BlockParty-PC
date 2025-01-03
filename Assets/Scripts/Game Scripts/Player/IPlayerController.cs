using UnityEngine;

public interface IPlayerController
{
    bool IsGrounded(Vector3 groundCheckPosition, float groundDistance, LayerMask groundMask);
    void ResetVerticalVelocity();
    void Jump(float jumpHeight, float gravity);
    Vector3 CalculateMoveDirection(Transform cameraTransform, float horizontal, float vertical);
    void RotateTowards(Vector3 direction, float rotationSpeed);
    void Move(Vector3 direction, float moveSpeed);

}