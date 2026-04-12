using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

public class RoundRewardManager : MonoBehaviourPunCallbacks
{
    public static RoundRewardManager Instance;

    private const int COINS_PER_ROUND = 2;
    private const int COINS_WIN_GAME  = 50;

    private int _roundsWon = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GameRoundManager.OnNextMapStarted += OnRoundCompleted;
        LevelPlayAds.onAdRewardAction     += OnAdRewarded;
    }

    private void OnDisable()
    {
        GameRoundManager.OnNextMapStarted -= OnRoundCompleted;
        LevelPlayAds.onAdRewardAction     -= OnAdRewarded;
    }

    private void OnAdRewarded(RewardType type)
    {
        switch (type)
        {
            case RewardType.Coins:
                PlayerDataManager.Instance?.AddCoins(50);
                break;
      
        }
    }

    private void OnRoundCompleted()
    {
        if (!IsLocalPlayerAlive()) return;

        _roundsWon++;
        PlayerDataManager.Instance?.AddCoins(COINS_PER_ROUND);
    }

    public void OnGameWon()
    {
        PlayerDataManager.Instance?.AddCoins(COINS_WIN_GAME);
    }

    private bool IsLocalPlayerAlive()
    {
        var localPlayer = PhotonNetwork.LocalPlayer;
        if (localPlayer == null) return false;

        if (localPlayer.CustomProperties.TryGetValue("isDead", out var v) && v is bool isDead)
            return !isDead;

        return true;
    }
}