using UnityEngine;

public abstract class Effect
{
    public EffectsEnum EffectEnum;
    public float Duration;

    public Effect(EffectsEnum effectEnum, float duration)
    {
        EffectEnum = effectEnum;
        Duration = duration;
    }
    
    public abstract void ApplyEffect(GameObject player);
    public abstract void RemoveEffect(GameObject player);
    
}