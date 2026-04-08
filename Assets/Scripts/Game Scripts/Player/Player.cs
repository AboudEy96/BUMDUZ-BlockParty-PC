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
            try
            {
                playerName.text = PhotonNetwork.LocalPlayer.NickName;
            }
            catch (System.Exception e)
            {
                playerName.text = "Player 1";
            }
            
        }
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            HandleMovement();
        }
    }

    public void HandleMovement()
    {
        float horizontal;
        float vertical;
        bool jumpPressed;

        #if UNITY_ANDROID || UNITY_IOS
            Vector2 mobileInput = MobileInputProvider.Instance != null
                ? MobileInputProvider.Instance.MoveInput
                : Vector2.zero;

            horizontal  = mobileInput.x;
            vertical    = mobileInput.y;
            jumpPressed = MobileInputProvider.Instance != null && MobileInputProvider.Instance.JumpPressed;
        #else
            horizontal  = Input.GetAxisRaw("Horizontal");
            vertical    = Input.GetAxisRaw("Vertical");
            jumpPressed = Input.GetButtonDown("Jump");
        #endif

        Vector3 moveDirection = characterController.CalculateMoveDirection(cameraTransform, horizontal, vertical);
        characterController.RotateTowards(moveDirection, rotationSpeed);
        characterController.Move(moveDirection, hasEffect("Speed") ? moveSpeed + 2 : moveSpeed);

        if (jumpPressed)
        {
            characterController.Jump(hasEffect("JumpBoost") ? jumpForce + 2f : jumpForce, gravity);
        }

        IsRunning = characterController.IsMoving;
    //    animator.SetBool("RUN", IsRunning);
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