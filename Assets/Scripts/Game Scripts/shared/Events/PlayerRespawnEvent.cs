using System.Collections;
using Photon.Pun;
using TMPro;
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

        foreach (PhotonView v in players)
        {
            if (v.CompareTag("Player") && v.IsMine)
            {
                player = v.gameObject;
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
        if (!IsPlayerRespawning()) return;

        DoRespawn();
    }

    public void OnPlayerQueueRespawn(RewardType type)
    {
        if (type != RewardType.Respawn) return;
        if (!IsPlayerDead()) return;

        Debug.Log("PlayerQueueRespawn");
        
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Single player: respawning immediately after ad.");
            view.Owner.SetCustomProperties(new Hashtable
            {
                { "isDead", false },
                { "Respawning", false }
            });
            DoRespawn();
            return;
        }

        view.Owner.SetCustomProperties(new Hashtable
        {
            { "Respawning", true }
        });
        respawnCam.gameObject.SetActive(true);
        deathScreen.gameObject.SetActive(false);
        title.text = "RESPAWNING ON NEXT ROUND..";
    }
    
    private void DoRespawn()
    {
        ShowPlayer();
        RespawnPlayerPosition();
        title.text = " ";
        StartCoroutine(RestoreGfx());
        
        if (view != null)
        {
            view.Owner.SetCustomProperties(new Hashtable
            {
                { "isDead", false },
                { "Respawning", false }
            });
        }

        respawnCam.gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(false);
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

        player.transform.position = new Vector3(x, 1, z);
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
