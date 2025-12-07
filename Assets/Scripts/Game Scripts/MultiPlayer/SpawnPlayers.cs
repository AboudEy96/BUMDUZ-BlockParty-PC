using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class SpawnPlayers : MonoBehaviour
{
    
    public GameObject PREFAB_PLAYER;
    public String PLAYER_SKIN;
    private string playerName = CreateJoinRooms.playerNameInLobby;
    public int height;
    public int width;
    public int yAXIS;

 //   [Header("Materials Skinsss to translate the string to material")]
//    public List<Material> _materialsSkins;

    private void Awake()
    {
        PLAYER_SKIN = PlayerPrefs.GetString("Skin", "PurpleBlue");
        Debug.Log(PlayerPrefs.GetString("Skin"));
        PREFAB_PLAYER = PlayerCharacterSingletoon.instance.CHARACTER;
        Debug.Log(PREFAB_PLAYER.name);
    }

    void Start()
    {

        int x = Random.Range(-18, width);
        int z = Random.Range(-15, height);
        Vector3 location = new Vector3(x, yAXIS, z);
        //  PhotonNetwork.Instantiate(player.name, location, Quaternion.identity);

        
        SkinnedMeshRenderer sms = PREFAB_PLAYER.GetComponentInChildren<SkinnedMeshRenderer>();
        
     /*   foreach (var mat in _materialsSkins)
        {
            if (mat.name == PLAYER_SKIN)
            {
                sms.GetComponentInChildren<SkinnedMeshRenderer>().material = mat;
                
                break;
            }
        }*/

        PlayerUser player = new PlayerUserBuilder(PREFAB_PLAYER)
            .SetId(1)
            .SetName(playerName)
            .Build(location);
            //.SetSkinMaterial(PLAYER_SKIN)
         

        
    }
}