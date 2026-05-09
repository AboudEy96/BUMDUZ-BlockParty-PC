using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float moveSpeed = 6.0f;
    private float rotationSpeed = 10.0f;

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

    [Header("Mobile Input")]
    public InputActionAsset inputActions;
    private InputAction _moveAction;

    private void Awake()
    {
        characterController = new PlayerController(GetComponent<CharacterController>());
        _photonView = GetComponent<PhotonView>();

        if (inputActions != null)
        {
            _moveAction = inputActions.FindActionMap("movement").FindAction("input");
        }
    }

    private void OnEnable()
    {
        _moveAction?.Enable();
    }

    private void OnDisable()
    {
        _moveAction?.Disable();
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
        if (!_photonView.IsMine)
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (!GetComponent<CharacterController>().enabled) return;

            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        float horizontal;
        float vertical;
        bool jumpPressed;

        #if UNITY_ANDROID || UNITY_IOS
            Vector2 moveInput = _moveAction != null ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
            horizontal  = moveInput.x;
            vertical    = moveInput.y;
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
    // this only to share the run animatinos between photon players
        animator.SetBool("RUN", IsRunning);
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