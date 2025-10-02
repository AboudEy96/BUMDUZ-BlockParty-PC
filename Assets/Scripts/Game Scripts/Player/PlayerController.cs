using Photon.Pun;
using UnityEngine;

public class PlayerController : IPlayerController
{
    private Vector3 _velocity;
    private readonly CharacterController _controller;
    private readonly Rigidbody _rb;
    private readonly PhotonView _photonView;
    public float speed;
    public PlayerController(CharacterController controller)
    {
        _controller = controller;
        if (_controller == null)
        {
            Debug.LogError("CharacterController is not assigned!");
        }
    }
    
    public PlayerController(Rigidbody rigidbody)
    {
        _rb = rigidbody;
        if (_rb == null)
        {
            Debug.LogError("Rigidbody is not assigned!");
        }
    }

    public bool IsGrounded(Vector3 groundCheckPosition, float groundDistance, LayerMask groundMask)
    {
        bool isGrounded = Physics.CheckSphere(groundCheckPosition, groundDistance, groundMask) && _velocity.y < 0;
      //  Debug.Log($"Is Grounded: {isGrounded}");
        return isGrounded;
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
        speed = moveSpeed;
        _controller.Move(direction * speed * Time.deltaTime);
        ApplyGravity();
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        _velocity.y += Physics.gravity.y * Time.deltaTime;
    }
}
