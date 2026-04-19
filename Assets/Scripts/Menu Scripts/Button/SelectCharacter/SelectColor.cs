using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectColor : MonoBehaviour
{
    [SerializeField] private Transform BUTTON_PARENT;
    [SerializeField] private Transform PANEL_OF_COLORS;
    [SerializeField] private GameObject[] Characters;
    public static SelectColor Instance;
    
    private SkinnedMeshRenderer _sms;
    private int _selectedCharacter;

    private void Start()
    {
        Instance = this;
        Button[] buttons = BUTTON_PARENT.GetComponentsInChildren<Button>();
        foreach (Button bt in buttons)
        {
            // Capture loop variable correctly
            Button captured = bt;
            captured.onClick.AddListener(() => ButtonClick(captured));
        }
    }
    private void OnEnable()
    {
        RefreshCharacterSelection();
        SyncCharacterColor();
        SortUnlockedSkins();
    }

      public void SortUnlockedSkins()
    {
        var buttons = PANEL_OF_COLORS.GetComponentsInChildren<Button>().ToList();

        var sorted = buttons.OrderBy(b =>
            !PlayerDataManager.Instance.IsSkinUnlocked($"{Characters[_selectedCharacter].name}[{b.name}]")
        ).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            var button = sorted[i];

            bool unlocked = PlayerDataManager.Instance.IsSkinUnlocked(
                $"{Characters[_selectedCharacter].name}[{button.name}]"
            );

            button.transform.SetSiblingIndex(i);
//            button.interactable = unlocked;
            var img = button.image;
            Color c = img.color;
            c.a = unlocked ? 1f : 0.4f; 
            img.color = c;
            
            var lockImage = button.transform.Find("Image");
            if (lockImage != null)
                lockImage.gameObject.SetActive(!unlocked);
        }
    }
    public void RefreshCharacterSelection()
    {
        _selectedCharacter = PlayerPrefs.GetInt("CharacterType", 0);
        
        if (Characters == null || Characters.Length == 0)
        {
            Debug.LogError("Characters array is empty or not assigned!");
            return;
        }

        GameObject selected = Characters[_selectedCharacter];
        if (selected == null)
        {
            Debug.LogError($"Characters[{_selectedCharacter}] is null!");
            return;
        }

        _sms = selected.GetComponentInChildren<SkinnedMeshRenderer>();
        if (_sms == null)
            Debug.LogError($"No SkinnedMeshRenderer found on {selected.name}");
    }

    public void ButtonClick(Button button)
    {
        // BUMDUZ[RED] Example
        if (PlayerDataManager.Instance.IsSkinUnlocked($"{Characters[_selectedCharacter].name}[{button.name}]"))
        {
            PlayerPrefs.SetString("Skin", button.name);
            SyncCharacterColor();
            Debug.Log("Skin selected: " + PlayerPrefs.GetString("Skin"));
            return;
        }
        Debug.Log("Skin is not available.");
        
    }

    public void SyncCharacterColor()
    {
        if (_sms == null)
        {
            Debug.LogError("_sms is null in SyncCharacterColor. Was RefreshCharacterSelection called?");
            return;
        }

        if (SyncPlayerMaterial.instance == null)
        {
            Debug.LogError("SyncPlayerMaterial.instance is null!");
            return;
        }

        string characterName = Characters[_selectedCharacter].name;
        string targetSkin = PlayerPrefs.GetString("Skin", "");

        List<Material> materials = SyncPlayerMaterial.instance.getCurrentMaterial(characterName);

        foreach (var mat in materials)
        {
            if (mat.name == targetSkin)
            {
                _sms.material = mat; 
                break;
            }
        }

        Debug.Log($"Current character index: {_selectedCharacter}, name: {characterName}");
    }
}