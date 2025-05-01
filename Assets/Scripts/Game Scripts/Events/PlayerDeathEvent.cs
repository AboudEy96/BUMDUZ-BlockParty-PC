using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerDeathEvent : MonoBehaviour
{
    [Header("Effect Volume")]
    public PostProcessVolume EFFECT;

    private ColorGrading colorGrading;

    private void Start()
    {
        // نحاول ناخذ إعدادات ColorGrading من الـ Volume
        if (!EFFECT.profile.TryGetSettings(out colorGrading))
        {
            Debug.LogError("ColorGrading not found in the PostProcessVolume!");
        }
    }

    public void OnVoidDeath(GameObject player)
    {
        StartCoroutine(ChangeGfxToBlack());
        HidePlayer(player);
    }

    public void HidePlayer(GameObject player)
    {
        foreach (GameObject child in player.transform)
        {
            if (child.CompareTag("Skin"))
            {
                child.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }   
        }
        
    }

    private IEnumerator ChangeGfxToBlack()
    {
        float elapsed = 0f;
        float duration = 2f;
        float startValue = colorGrading.saturation.value;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            colorGrading.saturation.value = Mathf.Lerp(startValue, -60f, t);
            yield return null;
        }

        colorGrading.saturation.value = -60f;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            OnVoidDeath(other.gameObject);
        }
    }
}