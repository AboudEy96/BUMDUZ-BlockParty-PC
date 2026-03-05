using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // MOVE FORCES
    private float moveSpeed = 6.0f;
    private float rotationSpeed = 10.0f;

    // JUMP FORCES
    private float jumpForce = 1.0f;
    private float gravity = -9.81f;

    private TextMeshPro playerName;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Camera Settings")]
    public Transform cameraTransform;

    private PlayerController characterController;
    public bool IsRunning { get; private set; }
    private ServerOnlinePlayers serverOnlinePlayers;
    PhotonView _photonView;

    [Header("Player Animator")] public Animator animator;

    private void Awake()
    {
        characterController = new PlayerController(GetComponent<CharacterController>());
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        playerName = GetComponentInChildren<TextMeshPro>();
        if (_photonView.IsMine)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
            }
            playerName.text = PhotonNetwork.LocalPlayer.NickName;
        }
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = characterController.CalculateMoveDirection(cameraTransform, Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        characterController.RotateTowards(moveDirection, rotationSpeed);
        characterController.Move(moveDirection, hasEffect("Speed") ? moveSpeed + 2 : moveSpeed);

        if (Input.GetButtonDown("Jump"))
        {
            characterController.Jump(hasEffect("JumpBoost") ? jumpForce + 2f : jumpForce, gravity);
        }

        IsRunning = characterController.IsMoving;
        animator.SetBool("Run", IsRunning);
    }   

    bool hasEffect(string childName)
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.name.StartsWith(childName))
            {
                return true;
            }
        }
        return false;
    }
}
