using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    private string currentState;

    private const string IDLE = "Idle";
    private const string RUN = "Run";
    private const string JUMP = "Jump";

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (characterController == null) characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        string targetState;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        bool isMoving = moveX != 0 || moveZ != 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetState = JUMP;
        }
        else if (isMoving)
        {
            targetState = RUN;
        }
        else
        {
            targetState = IDLE;
        }

        ChangeAnimationState(targetState);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.CrossFade(newState, 0.1f);
        currentState = newState;
    }
}