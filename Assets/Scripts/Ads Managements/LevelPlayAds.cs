using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.LevelPlay;

public class LevelPlayAds : MonoBehaviour
{
    [SerializeField] private Button adButton;
    private LevelPlayRewardedAd rewardedAd;

    private static bool isInitialized = false;

    private const string APP_KEY    = AdsConfig.APP_KEY;
    private const string AD_UNIT_ID = AdsConfig.AD_UNIT_ID;

    private void Start()
    {
        if (isInitialized)
        {
            Debug.LogWarning("LevelPlay: already initialized, skipping.");
            return;
        }

        try
        {
            isInitialized = true;

            LevelPlay.SetConsent(true);
            LevelPlay.SetMetaData("do_not_sell", "false");
            LevelPlay.SetMetaData("is_child_directed", "false");

            LevelPlay.OnInitSuccess += OnInitSuccess;
            LevelPlay.OnInitFailed  += OnInitFailed;

            LevelPlay.Init(APP_KEY);
            Debug.Log("LevelPlay: Init called");
        }
        catch (Exception e)
        {
            isInitialized = false;
            Debug.LogError($"LevelPlay: Init exception → {e.Message}");
        }
    }

    private void OnInitSuccess(LevelPlayConfiguration configuration)
    {
        Debug.Log("LevelPlay: SDK initialized successfully");

        rewardedAd = new LevelPlayRewardedAd(AD_UNIT_ID);

        rewardedAd.OnAdLoaded        += OnAdLoaded;
        rewardedAd.OnAdLoadFailed    += OnAdLoadFailed;
        rewardedAd.OnAdDisplayed     += OnAdDisplayed;
        rewardedAd.OnAdDisplayFailed += OnAdDisplayFailed;
        rewardedAd.OnAdClicked       += OnAdClicked;
        rewardedAd.OnAdClosed        += OnAdClosed;
        rewardedAd.OnAdRewarded      += OnAdRewarded;
        rewardedAd.OnAdInfoChanged   += OnAdInfoChanged;

        rewardedAd.LoadAd();
        Debug.Log("LevelPlay: LoadAd called");

        adButton.onClick.RemoveAllListeners();
        adButton.onClick.AddListener(() =>
        {
            if (rewardedAd != null && rewardedAd.IsAdReady())
            {
                Debug.Log("LevelPlay: ShowAd called");
                rewardedAd.ShowAd();
            }
            else
            {
                Debug.LogWarning("LevelPlay: Ad not ready yet!");
            }
        });
    }

    private void OnInitFailed(LevelPlayInitError error)
    {
        isInitialized = false;
        Debug.LogError($"LevelPlay: Init FAILED → {error}");
    }
    
    private void OnAdLoaded(LevelPlayAdInfo adInfo)
    {
        Debug.Log("LevelPlay: Ad loaded");
        adButton.gameObject.SetActive(true);
    }
    
    private void OnAdLoadFailed(LevelPlayAdError error)
    {
        Debug.LogError($"LevelPlay: Ad load failed → {error}");
    }
    
    private void OnAdDisplayed(LevelPlayAdInfo adInfo)
    {
        Debug.Log("LevelPlay: Ad displayed");
    }
    
    private void OnAdDisplayFailed(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        Debug.LogError($"LevelPlay: Ad display failed → {error}");
    }
    
    private void OnAdClicked(LevelPlayAdInfo adInfo)
    {
        Debug.Log("LevelPlay: Ad clicked");
    }
    
    private void OnAdClosed(LevelPlayAdInfo adInfo)
    {
        Debug.Log("LevelPlay: Ad closed");
        rewardedAd.LoadAd(); 
    }
    
    private void OnAdRewarded(LevelPlayAdInfo adInfo, LevelPlayReward reward)
    {
        Debug.Log($"LevelPlay: User rewarded → {reward.Name} x{reward.Amount}");
    }
    
    private void OnAdInfoChanged(LevelPlayAdInfo adInfo)
    {
        Debug.Log("LevelPlay: Ad info changed");
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.IsAdReady())
            rewardedAd.ShowAd();
    }

    private void OnDestroy()
    {
        isInitialized = false; 

        LevelPlay.OnInitSuccess -= OnInitSuccess;
        LevelPlay.OnInitFailed  -= OnInitFailed;

        if (rewardedAd != null)
        {
            rewardedAd.OnAdLoaded        -= OnAdLoaded;
            rewardedAd.OnAdLoadFailed    -= OnAdLoadFailed;
            rewardedAd.OnAdDisplayed     -= OnAdDisplayed;
            rewardedAd.OnAdDisplayFailed -= OnAdDisplayFailed;
            rewardedAd.OnAdClicked       -= OnAdClicked;
            rewardedAd.OnAdClosed        -= OnAdClosed;
            rewardedAd.OnAdRewarded      -= OnAdRewarded;
            rewardedAd.OnAdInfoChanged   -= OnAdInfoChanged;
        }
    }
}