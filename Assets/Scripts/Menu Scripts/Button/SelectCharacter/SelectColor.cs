using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectColor : MonoBehaviour
{
    [SerializeField] private Transform BUTTON_PARENT;
    [SerializeField] private GameObject[] Characters;

    private SkinnedMeshRenderer _sms;
    private int _selectedCharacter;

    private void Start()
    {

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
        PlayerPrefs.SetString("Skin", button.name);
        SyncCharacterColor();
        Debug.Log("Skin selected: " + PlayerPrefs.GetString("Skin"));
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