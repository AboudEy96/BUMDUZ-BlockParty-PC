using UnityEngine;

public class SpeedEffect : Effect
{
    private float mineSpeed;
    private float toAddSpeed;
    public SpeedEffect(float duration) : base(EffectsEnum.SPEED, duration) { }
    
    public override void ApplyEffect(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        mineSpeed = playerController.speed;
        playerController.speed += toAddSpeed;
    }

    public override void RemoveEffect(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.speed = mineSpeed;

    }
    
}