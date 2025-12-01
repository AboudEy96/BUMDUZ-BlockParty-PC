using Menu_Scripts.LobbyMenu.PlayersInRoom;
using Photon.Pun;
using UnityEngine;

public class PhotonPlayerFactory : MonoBehaviour, IPlayerFactory
{

    public Transform[] spawnPoints;
    public IPlayer CreatePlayer(int spawnIndex, SkinInfo skinInfo)
    {
        var go = PhotonNetwork.Instantiate(skinInfo.prefab.name,
            spawnPoints[spawnIndex].position, 
            spawnPoints[spawnIndex].rotation);
        var player = go.GetComponent<IPlayer>();
        player.Initialize(PhotonNetwork.LocalPlayer.ActorNumber, null);
        return player;
    }
}