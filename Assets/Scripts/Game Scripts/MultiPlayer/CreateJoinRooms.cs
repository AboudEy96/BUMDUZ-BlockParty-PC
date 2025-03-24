using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CreateJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField create;
    public InputField join;
    private Dictionary<string, int> roomList = new Dictionary<string, int>();
    
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(create.text);
        roomList.Add(create.text, 1);
        Debug.Log($"created room with name {create.text} with this type of players {roomList[create.text]}");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(join.text);
        roomList[join.text] += 1;
        Debug.Log($"some player joined to {join.text} with this type of players {roomList[join.text]}");
    }

    [PunRPC]
    public override void OnJoinedRoom()
    {
        ServerOnlinePlayers.addOnlinePlayer();
        PhotonNetwork.LoadLevel("Game");
        PhotonNetwork.LocalPlayer.NickName = $"Player-{PhotonNetwork.LocalPlayer.ActorNumber}";
    }
}