using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CreateJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField create;
    public InputField join;
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(create.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(join.text);
    }

    public override void OnJoinedRoom()
    {
        ServerOnlinePlayers.addOnlinePlayer();
        PhotonNetwork.LoadLevel("Game");
            PhotonNetwork.LocalPlayer.NickName = $"Player-{PhotonNetwork.LocalPlayer.ActorNumber}";
    }
}