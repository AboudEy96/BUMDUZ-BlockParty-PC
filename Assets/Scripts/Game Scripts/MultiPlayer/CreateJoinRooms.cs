using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CreateJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createRoomInput;
    public InputField joinRoomInput;
    public InputField playerNameInput;
    public static string playerNameInLobby;
    public Canvas canvas;
    public GameObject joinButtonPrefab;
    public Transform roomListContainer;
    [Header("Start Game Button for host master")]
    public GameObject startGameButton;

    private bool pendingCreateRoom = false;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("✅ Joined Lobby.");
        if (pendingCreateRoom)
        {
            CreateRoomNow();
            pendingCreateRoom = false;
        }
    }

    public void CreateRoom()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            pendingCreateRoom = true;
            return;
        }

        CreateRoomNow();
    }

    private void CreateRoomNow()
    {
        if (string.IsNullOrEmpty(createRoomInput.text))
        {
            Debug.LogWarning("❌ Room name cannot be empty.");
            return;
        }

        RoomOptions options = new RoomOptions
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 4
        };

        PhotonNetwork.CreateRoom(createRoomInput.text, options);
    }

    public override void OnRoomListUpdate(List<RoomInfo> updatedRoomList)
    {
        foreach (Transform child in roomListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo room in updatedRoomList)
        {
            if (room.RemovedFromList || !room.IsVisible || !room.IsOpen)
                continue;

            GameObject button = Instantiate(joinButtonPrefab, roomListContainer);
            button.transform.name = room.Name;

            string roomName = room.Name;

            button.GetComponentInChildren<Text>().text = roomName; 

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.JoinRoom(roomName);
                }
                else
                {
                    Debug.LogWarning("❌ You're not in the lobby yet. Please wait...");
                }
            });
        }
    }

    public void JoinRoom()
    {
        if (!PhotonNetwork.InLobby)
        {
            Debug.LogWarning("❌ You're not in the lobby yet. Please wait...");
            return;
        }

        if (string.IsNullOrEmpty(joinRoomInput.text))
        {
            Debug.LogWarning("❌ Join Room field is empty.");
            return;
        }

        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    public override void OnJoinedRoom()
    {
        if (string.IsNullOrEmpty(playerNameInput.text))
        {
            playerNameInput.text = "Player" + Random.Range(1000, 9999);
        }

        playerNameInLobby = playerNameInput.text;
        PhotonNetwork.LocalPlayer.NickName = playerNameInput.text;

        ServerOnlinePlayers.addOnlinePlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
            startGameButton.GetComponent<Button>().onClick.RemoveAllListeners();
            startGameButton.GetComponent<Button>().onClick.AddListener(StartGame);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("❌ Create Room Failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("❌ Join Room Failed: " + message);
    }
}
