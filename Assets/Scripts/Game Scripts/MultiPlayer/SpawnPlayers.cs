using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject PREFAB_PLAYER = PlayerCharacterSingletoon.CHARACTER;
    public String PLAYER_SKIN;
    private string playerName = CreateJoinRooms.playerNameInLobby;
    public int height;
    public int width;
    public int yAXIS;

    [Header("Materials Skinsss to translate the string to material")]
    public List<Material> _materialsSkins;

    private void Awake()
    {
        PlayerPrefs.SetString("Skin", "PurpleBlue");
        PLAYER_SKIN = PlayerPrefs.GetString("Skin", "PurpleBlue");
    }

    void Start()
    {

        int x = Random.Range(-19, width);
        int z = Random.Range(-21, height);
        Vector3 location = new Vector3(x, yAXIS, z);
        //  PhotonNetwork.Instantiate(player.name, location, Quaternion.identity);


        PlayerUser player = new PlayerUserBuilder(PREFAB_PLAYER)
            .SetId(1)
            .SetName(playerName)
            .SetSkinMaterial(PLAYER_SKIN)
            .Build(location);

        SkinnedMeshRenderer sms = player.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();


        foreach (var mat in _materialsSkins)
        {
            if (mat.name == PLAYER_SKIN)
            {
                sms.material = mat;
                break;
            }
        }
    }
}