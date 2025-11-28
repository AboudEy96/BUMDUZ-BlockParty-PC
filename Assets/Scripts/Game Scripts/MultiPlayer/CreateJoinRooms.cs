using System.Collections;
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
        
        Debug.Log("🔹 Start called");
    //    PhotonNetwork.JoinLobby(TypedLobby.Default);
        // awrp players when game starts
       // PhotonNetwork.AutomaticallySyncScene = true;
        
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("🔹 Not connected. Connecting using settings...");
            PhotonNetwork.ConnectUsingSettings();
        }
     /*   else if (!PhotonNetwork.InLobby)
        {
            StartCoroutine(StartJoinLobby());
        }*/
    }
    private IEnumerator StartJoinLobby()
    {
        yield return new WaitForSeconds(0.3f);
        
            PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("✅ Connected to Master. Joining lobby...");
    PhotonNetwork.JoinLobby(TypedLobby.Default);

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("✅ Joined Lobby.");
        if (pendingCreateRoom)
        {
            Debug.Log("🔹 Pending room creation detected. Creating room now...");
            CreateRoomNow();
            pendingCreateRoom = false;
        }
    }

    public void CreateRoom()
    {
        
        Debug.Log("🔹 CreateRoom called");
        if (!PhotonNetwork.InLobby)
        {
            Debug.LogWarning("⚠️ Not in lobby yet. Joining lobby first...");
            PhotonNetwork.JoinLobby();
            pendingCreateRoom = true;
            return;
        }

        CreateRoomNow();
    }

    private void CreateRoomNow()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
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

        Debug.Log("🔹 Creating room: " + createRoomInput.text);
        PhotonNetwork.CreateRoom(createRoomInput.text, options);
    }

    public override void OnRoomListUpdate(List<RoomInfo> updatedRoomList)
    {
        Debug.Log("🔹 OnRoomListUpdate called. Rooms count: " + updatedRoomList.Count);

        if (roomListContainer == null)
        {
            Debug.LogError("❌ roomListContainer is NULL!");
            return;
        }

        foreach (Transform child in roomListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo room in updatedRoomList)
        {
            Debug.Log("🔹 Room found: " + room.Name + " | Open: " + room.IsOpen + " | Visible: " + room.IsVisible);

            if (room.RemovedFromList || !room.IsVisible || !room.IsOpen)
            {
                Debug.Log("⚠️ Skipping room: " + room.Name);
                continue;
            }

            if (joinButtonPrefab == null)
            {
                Debug.LogError("❌ joinButtonPrefab is NULL! Cannot instantiate join button.");
                continue;
            }

            GameObject button = Instantiate(joinButtonPrefab, roomListContainer);
            if (button == null)
            {
                Debug.LogError("❌ Failed to instantiate button prefab.");
                continue;
            }

            button.name = room.Name;

            var textComponent = button.GetComponentInChildren<Text>();
            if (textComponent == null)
            {
                Debug.LogError("❌ No Text component found in joinButtonPrefab (" + button.name + ")");
            }
            else
            {
                textComponent.text = room.Name;
                Debug.Log("✅ Button text set to: " + room.Name);
            }

            var buttonComp = button.GetComponent<Button>();
            if (buttonComp == null)
            {
                Debug.LogError("❌ No Button component found in joinButtonPrefab (" + button.name + ")");
            }
            else
            {
                buttonComp.onClick.AddListener(() =>
                {
                    Debug.Log("🔹 Button clicked for room: " + room.Name);
                    if (PhotonNetwork.InLobby)
                    {
                        PhotonNetwork.JoinRoom(room.Name);
                    }
                    else
                    {
                        Debug.LogWarning("❌ You're not in the lobby yet. Please wait...");
                    }
                });
            }
        }
    }

    public void JoinRoom()
    {
        
        Debug.Log("🔹 JoinRoom called");
        PhotonNetwork.AutomaticallySyncScene = true;
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

        Debug.Log("🔹 Joining room: " + joinRoomInput.text);
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("🔹 OnJoinedRoom called");

        if (playerNameInput == null)
        {
            Debug.LogError("❌ playerNameInput is NULL!");
            playerNameInput.text = "Player" + Random.Range(1000, 9999);
        }
        else if (string.IsNullOrEmpty(playerNameInput.text))
        {
            playerNameInput.text = "Player" + Random.Range(1000, 9999);
        }

        playerNameInLobby = playerNameInput.text;
        PhotonNetwork.LocalPlayer.NickName = playerNameInLobby;
        Debug.Log("✅ Player name set to: " + playerNameInLobby);

        try
        {
            ServerOnlinePlayers.addOnlinePlayer();
            Debug.Log("✅ ServerOnlinePlayers.addOnlinePlayer() called successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ Error calling ServerOnlinePlayers.addOnlinePlayer(): " + e);
        }

        if (startGameButton == null)
        {
            Debug.LogError("❌ startGameButton is NULL!");
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
            startGameButton.GetComponent<Button>().onClick.RemoveAllListeners();
            startGameButton.GetComponent<Button>().onClick.AddListener(StartGame);
            Debug.Log("✅ startGameButton enabled for master client");
        }
        else
        {
            startGameButton.SetActive(false);
            Debug.Log("🔹 Not master client, startGameButton hidden");
        }
    }

    public void StartGame()
    {
        Debug.Log("StartGame called");
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("only master client can start the game");
            return;
        }
        StartCoroutine(DelayedStartGame());

    }

    private IEnumerator DelayedStartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.AutomaticallySyncScene = true;

        yield return new WaitForSeconds(1f);

        Debug.Log("Loading 'Game' via PhotonNetwork...");
        PhotonNetwork.LoadLevel("Game");

// chatgpt code if player did not move to game scene force moev
        yield return new WaitForSeconds(0.5f);
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Game")
        {
            Debug.LogWarning("Photon load canceled, forcing local scene load...");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
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
