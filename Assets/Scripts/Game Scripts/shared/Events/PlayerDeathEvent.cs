using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerDeathEvent : MonoBehaviourPunCallbacks
{
    [Header("Effect Volume")]
    public PostProcessVolume EFFECT;

    [Header("Death Canvas")]
    public Canvas DeathScreen;

    [Header("Single Player Settings")]
    [SerializeField] private float singlePlayerLeaveDelay = 10f;

    private ColorGrading colorGrading;
    private Coroutine _singlePlayerLeaveCoroutine;

    private void Start()
    {
        if (!EFFECT.profile.TryGetSettings(out colorGrading))
        {
            Debug.LogError("ColorGrading not found in the PostProcessVolume!");
        }
    }

    private void OnEnable()
    {
        LevelPlayAds.onAdRewardAction += OnAdRewarded;
    }

    private void OnDisable()
    {
        LevelPlayAds.onAdRewardAction -= OnAdRewarded;
    }

    private void OnAdRewarded(RewardType type)
    {
        if (type != RewardType.Respawn) return;
        if (_singlePlayerLeaveCoroutine != null)
        {
            StopCoroutine(_singlePlayerLeaveCoroutine);
            _singlePlayerLeaveCoroutine = null;
        }
    }

    public void OnVoidDeath(GameObject player)
    {
        StartCoroutine(ChangeGfxToBlack());
        HidePlayer(player);
        DeathScreen.gameObject.SetActive(true);
    }

    public void KillPlayerInPhoton(Photon.Realtime.Player owner)
    {
        if (owner == null)
            return;

        if (owner.CustomProperties.TryGetValue("isDead", out var value) && value is bool isDead && isDead)
            return;
        
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Single player marking isDead");

            owner.SetCustomProperties(new Hashtable
            {
                { "isDead", true }
            });
            
            _singlePlayerLeaveCoroutine = StartCoroutine(SinglePlayerLeaveAfterDelay());
            return;
        }

        owner.SetCustomProperties(new Hashtable
        {
            { "isDead", true }
        });

        if (PhotonNetwork.IsMasterClient && PlayerWinEvent.Instance != null)
        {
            PlayerWinEvent.Instance.CheckIfPlayerWin();
        }
    }

    private IEnumerator SinglePlayerLeaveAfterDelay()
    {
        yield return new WaitForSeconds(singlePlayerLeaveDelay);
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Single player: no respawn chosen, leaving room.");
            PhotonNetwork.LeaveRoom();
        }
    }

    public void HidePlayer(GameObject player)
    {
        foreach (Transform child in player.transform)
        {
            if (child.CompareTag("Skin"))
            {
                var r = child.GetComponent<SkinnedMeshRenderer>();
                if (r != null) r.enabled = false;
            }
        }
    }

    private IEnumerator ChangeGfxToBlack()
    {
        float elapsed = 0f;
        float duration = 2f;
        float startValue = colorGrading != null ? colorGrading.saturation.value : 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (colorGrading != null)
                colorGrading.saturation.value = Mathf.Lerp(startValue, -60f, t);

            yield return null;
        }

        if (colorGrading != null)
            colorGrading.saturation.value = -60f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name);

        if (!other.CompareTag("Player"))
            return;

        PhotonView view = other.GetComponent<PhotonView>();
        if (view != null && view.IsMine)
        {
            OnVoidDeath(other.gameObject);
            KillPlayerInPhoton(view.Owner);
        }
    }
}
