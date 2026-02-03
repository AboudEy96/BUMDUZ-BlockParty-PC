using Menu_Scripts.LobbyMenu.PlayersInRoom;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PhotonPlayerFactory : MonoBehaviour, IPlayerFactory
{

    public Transform[] spawnPoints;
    
    
    public IPlayer CreatePlayer(int spawnIndex, GameObject playerCharacter, Material skinInfo)
    {
        var go = PhotonNetwork.Instantiate(playerCharacter.name,
            spawnPoints[spawnIndex].position, 
            spawnPoints[spawnIndex].rotation);
        var player = go.GetComponent<IPlayer>();
        player.Initialize(PhotonNetwork.LocalPlayer.ActorNumber, playerCharacter, skinInfo);
        go.GetComponentInChildren<Renderer>().material = skinInfo;
        return player;
    }
}