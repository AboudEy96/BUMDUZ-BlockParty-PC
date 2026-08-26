using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EffectsManager : MonoBehaviour
{

    private List<Effect> activeEffects = new List<Effect>();

    public void ApplyEffect(Effect effect, GameObject player)
    {
        effect.ApplyEffect(player);
        activeEffects.Add(effect);
        StartCoroutine(RemoveEffectAfterDuration(effect, player));
    }

    private IEnumerator RemoveEffectAfterDuration(Effect effect, GameObject player)
    {
        yield return new WaitForSeconds(effect.Duration);
        effect.RemoveEffect(player);
        activeEffects.Remove(effect);
    }
    
}