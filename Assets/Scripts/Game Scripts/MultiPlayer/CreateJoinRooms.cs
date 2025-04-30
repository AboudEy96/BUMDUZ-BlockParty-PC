using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField create;
    public InputField join;
    public InputField playerName;
    public static string playerNameInLobby;
    public Canvas canvas;
    public GameObject joinButton;
    public Transform listContainer;
    public Transform Content;
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(create.text); 
    }

    public override void OnRoomListUpdate(List<RoomInfo> updatedRoomList)
    {
   /*     foreach (Transform child in listContainer)
        {
            Destroy(child.gameObject);
        }*/

        foreach (RoomInfo room in updatedRoomList)
        {
            if (room.RemovedFromList || !room.IsVisible || !room.IsOpen)
                continue;

            GameObject button = Instantiate(joinButton, listContainer);
       //    button.transform.GetComponentInChildren<TextMeshProUGUI>().text = room.Name;
            button.transform.name = room.Name;
            button.GetComponent<Button>().onClick.AddListener(() => {
                PhotonNetwork.JoinRoom(room.Name); 
            });
        }
    }
    
    
    [PunRPC]
    void CreateRoomButton(string roomName)
    {
        Room room = new Room(roomName, 123, "Unknown");
        
        PhotonNetwork.CreateRoom(room.GetRoomName());
       GameObject theButtonofRoom = PhotonNetwork.Instantiate(joinButton.name, Vector3.zero, Quaternion.identity);
       theButtonofRoom.transform.name = room.GetRoomName();
        theButtonofRoom.transform.SetParent(Content, false);
        
    }

    public void JoinRoom()
    {
        
        PhotonNetwork.JoinRoom(join.text);
    }
    

    [PunRPC]
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
     //       photonView.RPC("CreateRoomButton", RpcTarget.All, create.text);
        }
        ServerOnlinePlayers.addOnlinePlayer();
        PhotonNetwork.LoadLevel("Game");
        playerNameInLobby = playerName.text;
        PhotonNetwork.LocalPlayer.NickName = playerName.text;
    }
    
}