using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CreateJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField create;
    public InputField join;

    public GameObject joinButton;
    public Transform listContainer;
    private Dictionary<string, int> roomList = new Dictionary<string, int>();
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(create.text); 
        photonView.RPC("CreateRoomButton", RpcTarget.All, create.text);
    }

    [PunRPC]
    void CreateRoomButton(string roomName)
    {
        Room room = new Room(roomName, 123, "Unknown");
    //    PhotonNetwork.CreateRoom(room.GetRoomName());
        GameObject theButtonofRoom = PhotonView.Instantiate(joinButton, listContainer);
        theButtonofRoom.transform.name = room.GetRoomName();
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinButton.name);
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