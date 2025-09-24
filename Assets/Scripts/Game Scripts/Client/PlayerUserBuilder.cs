using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerUserBuilder
{
    private string _name;
    private int _id;
    private string _skinMaterial;
    private GameObject _playerPrefab;
    public PlayerUserBuilder(GameObject prefab)
    {
        _playerPrefab = prefab;
    }

    public PlayerUserBuilder SetName(string name)
    {
        _name = name;
        return this;

    }

    public PlayerUserBuilder SetId(int id)
    {
        _id = id;
        return this;

    }

    public PlayerUserBuilder SetSkinMaterial(string skinMaterial)
    {
        _skinMaterial = skinMaterial;
        return this;

    }
    

    public PlayerUserBuilder SetPlayerPrefab(GameObject playerPrefab)
    {
        _playerPrefab = playerPrefab;
        return this;

    }
    public PlayerUser Build(Vector3 spawnPosition)
    {
        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, spawnPosition, Quaternion.identity);
        PlayerUser playerComponent = player.GetComponent<PlayerUser>();
        SkinnedMeshRenderer sms = player.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        
        
    //    sms.material = _skinMaterial;
        playerComponent.SetupPlayer(_name, _id, _skinMaterial, _playerPrefab);
        // change the material to _skinMaterial and show it to all players 
        
        /* foreach (Transform skin in player.transform)
        {
            if (skin.gameObject.CompareTag("Skin"))
            {
                PhotonNetwork.Destroy(skin.gameObject);
                GameObject NewSkin =PhotonNetwork.Instantiate(_skinMaterial.name, player.transform.localPosition, Quaternion.identity);
                NewSkin.transform.parent = _playerPrefab.transform;
            }
        }*/
        return playerComponent;
    }
}