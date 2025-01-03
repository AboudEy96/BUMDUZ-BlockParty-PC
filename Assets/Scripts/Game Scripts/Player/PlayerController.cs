using UnityEngine;

public class PlayerController : IPlayerController
{
    private Vector3 _velocity;
    private readonly CharacterController _controller;

    public PlayerController(CharacterController controller)
    {
        _controller = controller;
    }
    public bool IsGrounded(Vector3 groundCheckPosition, float groundDistance, LayerMask groundMask)
    {
        return Physics.CheckSphere(groundCheckPosition, groundDistance, groundMask) && _velocity.y < 0;
    }

    public void ResetVerticalVelocity()
    {
        _velocity.y = -2f;
        
    }

    public void Jump(float jumpHeight, float gravity)
    {
        _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public Vector3 CalculateMoveDirection(Transform cameraTransform, float horizontal, float vertical)
    {
        Vector3 direction = cameraTransform.right * horizontal + cameraTransform.forward * vertical;
        direction.y = 0;
        return direction.normalized;
    }
    public void RotateTowards(Vector3 direction, float rotationSpeed)
    {
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _controller.transform.rotation = Quaternion.Lerp(_controller.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void Move(Vector3 direction, float moveSpeed)
    {
        _controller.Move(direction * moveSpeed * Time.deltaTime);
        ApplyGravity();
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        _velocity.y += Physics.gravity.y * Time.deltaTime;
    }
}