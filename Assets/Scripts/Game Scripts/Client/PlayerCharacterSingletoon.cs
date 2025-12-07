using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterSingletoon : MonoBehaviour
{
    // -- Singletoon to save the player character and color --
    public GameObject CHARACTER;
    public static PlayerCharacterSingletoon instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
      
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    
    public PlayerCharacterSingletoon GetInstance()
    {
        return instance;
    }
    public void SetCharacter(GameObject character)
    {
        CHARACTER = character;
    }

    public GameObject GetCharacter()
    {
        return CHARACTER;
    }

    
}