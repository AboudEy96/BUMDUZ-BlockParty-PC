using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;


public class ProfileUIData : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private Canvas _profileCanvas;
    
    [Header("Player Variables")]
    [SerializeField] private TMP_Text _playerLevel;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _playerUUID;
    [SerializeField] private TMP_Text _playerCoins;
    [SerializeField] private TMP_Text _playerWins;
    [SerializeField] private TMP_Text _playerPlayedGames;
    
    [Header("The Image, and Images")] 
    [SerializeField]private Image _currentImage;
    [SerializeField]private Sprite[] _profileImages;

    [Header("Logout button X Button")]
    [SerializeField] private Button _logoutButton;
    [SerializeField] private Button _closeButton;
    async void OnEnable()
    {
        await CloudSaveManager.Instance.LoadData();

        _playerName.text = SetTextColor(PlayerDataManager.Instance.GetPlayerName());
        _playerPlayedGames.text = $"Played:      [{PlayerDataManager.Instance.GetPlayedGames()}]";
        _playerWins.text = $"Wins:        [{PlayerDataManager.Instance.GetWins()}]";
        int coins = PlayerDataManager.Instance.GetCoins();
        
        _playerCoins.text = $"{coins}";
        _playerUUID.text = $"UUID: {AuthenticationService.Instance.PlayerId}";
        _currentImage.sprite = GetImage();
        _logoutButton.onClick.AddListener(() =>
        {
            AuthManager.Instance.Logout();
            AuthUIManager.Instance.ShowAuth();
            CloseProfile();
        });
        _closeButton.onClick.AddListener(CloseProfile);
    }

    private void OnDisable()
    {
        _logoutButton.onClick.RemoveAllListeners();
    }

    void CloseProfile()
    {
        _profileCanvas.gameObject.SetActive(false);
    }
    private Sprite GetImage()
    {
        int char_id = PlayerPrefs.GetInt("CharacterType");
        return _profileImages[char_id];
    }

    private string SetTextColor(string text)
    {
        int char_id = PlayerPrefs.GetInt("CharacterType");
        switch (char_id)
        {
            case 1:
                return $"<color=#e384d5>{text}</color>";
        }
        return $"<color=#f0ee86>{text}</color>";
    }
}