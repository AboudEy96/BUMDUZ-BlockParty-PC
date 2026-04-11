using System.Collections;
using Photon.Pun;
using TMPro;
using Unity.ProjectAuditor.Editor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerRespawnEvent : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Death Canvas")]
    [SerializeField] private Canvas deathScreen;

    [Header("Effect Volume")]
    [SerializeField] private PostProcessVolume effect;

    [Header("Waiting To Respawn Title")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Camera respawnCam;
    private ColorGrading colorGrading;

    private PhotonView view;
    private void Start()
    {
        if (!effect.profile.TryGetSettings(out colorGrading))
        {
            Debug.LogError("ColorGrading not found in the PostProcessVolume!");
        }
        FindLocalPlayer();
        view = player.GetComponent<PhotonView>();
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
        LevelPlayAds.onAdRewardAction += OnPlayerQueueRespawn;
        GameRoundManager.OnNextMapStarted += OnPlayerRespawn;
    }

    private void OnDisable()
    {
        LevelPlayAds.onAdRewardAction -= OnPlayerQueueRespawn;
        GameRoundManager.OnNextMapStarted -= OnPlayerRespawn;
    }

    public void OnPlayerRespawn()
    {
        var pl = view.Owner;
        if (!IsPlayerRespawning()) return;
        
        ShowPlayer();
        RespawnPlayerPosition();
        title.text = " ";
        StartCoroutine(RestoreGfx());
        pl.SetCustomProperties(new Hashtable
        {
            { "isDead", false }, 
            {"Respawning", false} 
        });
        respawnCam.gameObject.SetActive(false);
    }

    public void OnPlayerQueueRespawn(RewardType type)
    {
        if (type != RewardType.Respawn) return;
        if (!IsPlayerDead()) return;

        view.Owner.SetCustomProperties(new Hashtable
        {
            { "Respawning", true }
        });
        Debug.Log("PlayerQueueRespawn");
        respawnCam.gameObject.SetActive(true);
        deathScreen.gameObject.SetActive(false);
        title.text = "RESPAWNING ON NEXT ROUND..";
    }
    
    private bool IsPlayerDead()
    {
        if (view != null && view.Owner.CustomProperties.TryGetValue("isDead", out var value))
        {
            return (bool)value;
        }

        return false;
    }

    private bool IsPlayerRespawning()
    {
        if (view != null && view.Owner.CustomProperties.TryGetValue("Respawning", out var value))
        {
            return (bool)value;
        }

        return false;
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