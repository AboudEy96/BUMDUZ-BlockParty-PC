using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    private int _type;

    private string CHAR_KEY = "CharacterType";
    [SerializeField] private GameObject[] _character;
    [SerializeField] private Transform _spawnPoints;
    [SerializeField] private Button[] _buttons;
    
    void Start()
    {
        foreach (var button in _buttons)
        {
            button.onClick.AddListener(button.name.Contains("MUMDUZ") ? MUMDUZ : BUMDUZ);
        
            button.onClick.AddListener(ChangeCharacter);
        }
    }
    void OnEnable()
    {
        this._type = PlayerPrefs.GetInt(CHAR_KEY, 0);
        ChangeCharacter();
    }
    public void BUMDUZ()
    {
        // set the id to 0
        _type = 0;
        PlayerPrefs.SetInt(CHAR_KEY, 0);
        PlayerPrefs.Save();
        Debug.Log("Changed to BUMDUZ");
    }

    public void MUMDUZ()
    {
        _type = 1;
        PlayerPrefs.SetInt(CHAR_KEY, 1);
        PlayerPrefs.Save();
        Debug.Log("Changed to MUMDUZ");
        //set the id to 1
    }

    public void ChangeCharacter()
    {
    
        for (int i = 0; i < _character.Length; i++)
        {
            _character[i].SetActive(_type == i);
        }
    }
    
}
