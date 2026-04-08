using UnityEngine;

public class MobileInputProvider : MonoBehaviour
{
    public static MobileInputProvider Instance;

    public Vector2 MoveInput   { get; private set; }
    public bool    JumpPressed  { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetMoveInput(Vector2 input)  => MoveInput   = input;
    public void SetJumpPressed(bool value)   => JumpPressed = value;
}