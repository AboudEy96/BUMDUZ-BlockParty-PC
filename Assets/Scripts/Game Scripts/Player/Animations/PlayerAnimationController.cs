using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    private PhotonView _photonView;

    private string currentState;

    private const string IDLE = "Idle";
    private const string RUN  = "Run";
    private const string JUMP = "Jump";

    [Header("Mobile Input")]
    public InputActionAsset inputActions;
    private InputAction _moveAction;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (characterController == null) characterController = GetComponent<CharacterController>();
        _photonView = GetComponent<PhotonView>();

        if (inputActions != null)
            _moveAction = inputActions.FindActionMap("movement").FindAction("input");
    }

    private void OnEnable()  => _moveAction?.Enable();
    private void OnDisable() => _moveAction?.Disable();

    private void Update()
    {
        if (_photonView != null && !_photonView.IsMine) return;

        float moveX;
        float moveZ;
        bool jumpPressed;

        #if UNITY_ANDROID || UNITY_IOS
            Vector2 moveInput = _moveAction != null ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
            moveX       = moveInput.x;
            moveZ       = moveInput.y;
            jumpPressed = MobileInputProvider.Instance != null && MobileInputProvider.Instance.JumpPressed;
        #else
            moveX       = Input.GetAxisRaw("Horizontal");
            moveZ       = Input.GetAxisRaw("Vertical");
            jumpPressed = Input.GetKeyDown(KeyCode.Space);
        #endif

        bool isMoving = moveX != 0 || moveZ != 0;

        string targetState;

        if (jumpPressed)
            targetState = JUMP;
        else if (isMoving)
            targetState = RUN;
        else
            targetState = IDLE;

        ChangeAnimationState(targetState);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.CrossFade(newState, 0.1f);
        currentState = newState;
    }
}