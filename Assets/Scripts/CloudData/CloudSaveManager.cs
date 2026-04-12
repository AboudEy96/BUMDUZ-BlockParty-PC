using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

public class CloudSaveManager : MonoBehaviour
{
    public static CloudSaveManager Instance;

    private const string KEY_COINS = "coins";
    private const string KEY_NAME  = "playerName";
    private const string KEY_SKINS = "unlockedSkins";
    private const string KEY_PLAYED = "playedGames";
    private const string KEY_WINS   = "wins";
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async Task SaveData()
    {
        try
        {
            var data = new Dictionary<string, object>
            {
                { KEY_COINS, PlayerDataManager.Instance.GetCoins() },
                { KEY_NAME,  PlayerDataManager.Instance.GetPlayerName() },
                { KEY_SKINS, string.Join(",", PlayerDataManager.Instance.GetUnlockedSkins()) },
                { KEY_PLAYED, PlayerDataManager.Instance.GetPlayedGames() },
                { KEY_WINS,   PlayerDataManager.Instance.GetWins() }
            };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("Cloud Save: data saved");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Cloud Save failed: " + e.Message);
        }
    }

    public async Task LoadData()
    {
        try
        {
            var keys = new HashSet<string> 
            { 
                KEY_COINS, KEY_NAME, KEY_SKINS, KEY_PLAYED, KEY_WINS 
            };
            
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

            if (data.TryGetValue(KEY_COINS, out var coins))
                PlayerDataManager.Instance.SetCoins(int.Parse(coins.Value.GetAsString()));

            if (data.TryGetValue(KEY_NAME, out var name))
                PlayerDataManager.Instance.SetPlayerName(name.Value.GetAsString());
            
            if (data.TryGetValue(KEY_PLAYED, out var played))
                PlayerDataManager.Instance.SetPlayedGames(int.Parse(played.Value.GetAsString()));

            if (data.TryGetValue(KEY_WINS, out var wins))
                PlayerDataManager.Instance.SetWins(int.Parse(wins.Value.GetAsString()));
            
            
            if (data.TryGetValue(KEY_SKINS, out var skins))
            {
                string raw = skins.Value.GetAsString();
                if (!string.IsNullOrEmpty(raw))
                {
                    foreach (var skin in raw.Split(','))
                        PlayerDataManager.Instance.UnlockSkinFree(skin);
                }
            }

            Debug.Log("Cloud Save: data loaded");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Cloud Load failed: " + e.Message);
        }
    }
}
