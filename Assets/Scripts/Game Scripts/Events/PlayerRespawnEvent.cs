using System.Collections;
using Photon.Pun;
using Unity.ProjectAuditor.Editor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerRespawnEvent : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Death Canvas")]
    [SerializeField] private Canvas deathScreen;

    [Header("Effect Volume")]
    [SerializeField] private PostProcessVolume effect;

    private ColorGrading colorGrading;

    private void Start()
    {
        if (!effect.profile.TryGetSettings(out colorGrading))
        {
            Debug.LogError("ColorGrading not found in the PostProcessVolume!");
        }
        FindLocalPlayer();
    }
    private void FindLocalPlayer()
    {
        PhotonView[] players = FindObjectsOfType<PhotonView>();

        foreach (PhotonView view in players)
        {
            if (view.CompareTag("Player") && view.IsMine)
            {
                player = view.gameObject;
                Debug.Log("Local player found: " + player.name);
                return;
            }
        }
        Debug.LogError("Local player not found!");
    }

    private void OnEnable()
    {
        LevelPlayAds.onAdRewardAction += OnPlayerRespawn;
    }

    private void OnDisable()
    {
        LevelPlayAds.onAdRewardAction -= OnPlayerRespawn;
    }

    public void OnPlayerRespawn()
    {
        ShowPlayer();
        RespawnPlayerPosition();
        deathScreen.gameObject.SetActive(false);

        StartCoroutine(RestoreGfx());
    }

    private void ShowPlayer()
    {
        foreach (Transform child in player.transform)
        {
            if (child.CompareTag("Skin"))
            {
                var r = child.GetComponent<SkinnedMeshRenderer>();
                if (r != null)
                    r.enabled = true;
            }
        }
    }

    private void RespawnPlayerPosition()
    {
        int x = Random.Range(-18, 13);
        int z = Random.Range(-15, 17);

        player.transform.position = new Vector3(x,1,z);
    }

    private IEnumerator RestoreGfx()
    {
        float elapsed = 0f;
        float duration = 2f;

        float startValue = colorGrading != null
            ? colorGrading.saturation.value
            : -60f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (colorGrading != null)
                colorGrading.saturation.value = Mathf.Lerp(startValue, 0f, t);

            yield return null;
        }

        if (colorGrading != null)
            colorGrading.saturation.value = 0f;
    }
}