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
    
    public void SetJumpPressed() => JumpPressed = true;
    

    public bool CancelRepeatJump()
    {
        // if not pressed then return false to press if pressed return to press
        if (!JumpPressed) 
            return false;

        JumpPressed = false;
        return true;
    }
}