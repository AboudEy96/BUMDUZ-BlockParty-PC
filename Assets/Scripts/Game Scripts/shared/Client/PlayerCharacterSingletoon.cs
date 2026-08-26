using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterSingletoon : MonoBehaviour
{
    // -- Singletoon to save the player character and color --
    public GameObject[] LOBBY_CHARACTER; // without player scripts
    public GameObject[] GAME_CHARACTER; // with playtey scripts
    
    public static PlayerCharacterSingletoon instance;

    
    //=0-=-=-=-
    // 0 BUMDUZ
    // 1 MUMDUZ
    // -=--=-=-
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
    public void SetGameCharacter(int id, GameObject character)
    {
        GAME_CHARACTER[id] = character;
    }

    public GameObject GetGameCharacter(int id)
    {
        return GAME_CHARACTER[id];
    }

    
}