using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    private int          _coins;
    private string       _playerName = "Player";
    private List<string> _unlockedSkins = new List<string>();
    private int _playedGames;
    private int _wins;

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

    #region Coins

    public int GetCoins() => _coins;

    public void SetCoins(int amount)
    {
        _coins = amount;
    }

    public async void AddCoins(int amount)
    {
        _coins += amount;
        await CloudSaveManager.Instance.SaveData();
    }

    public async void SpendCoins(int amount)
    {
        if (_coins < amount) return;
        _coins -= amount;
        await CloudSaveManager.Instance.SaveData();
    }

    #endregion

    #region Wins and played Games
    #region Stats

    public int GetPlayedGames() => _playedGames;
    public int GetWins() => _wins;

    public void SetPlayedGames(int amount)
    {
        _playedGames = amount;
    }

    public void SetWins(int amount)
    {
        _wins = amount;
    }

    public async void AddPlayedGame()
    {
        _playedGames++;
        await CloudSaveManager.Instance.SaveData();
    }

    public async void AddWin()
    {
        _wins++;
        await CloudSaveManager.Instance.SaveData();
    }

    #endregion
    #endregion
    
    #region Player Name

    public string GetPlayerName() => _playerName;

    public void SetPlayerName(string name)
    {
        _playerName = name;
    }

    #endregion

    #region Skins

    public List<string> GetUnlockedSkins() => _unlockedSkins;

    public bool IsSkinUnlocked(string skinName) => _unlockedSkins.Contains(skinName);

    public async void UnlockSkin(string skinName, int price)
    {
        if (_unlockedSkins.Contains(skinName)) return;
        if (_coins < price) return;

        _coins -= price;
        _unlockedSkins.Add(skinName);
        await CloudSaveManager.Instance.SaveData();
    }

    public void UnlockSkinFree(string skinName)
    {
        if (_unlockedSkins.Contains(skinName)) return;
        _unlockedSkins.Add(skinName);
    }

    #endregion
}
