using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateJoinRooms : MonoBehaviourPunCallbacks
{
    #region === UI References ===
    [Header("Inputs")]
    [SerializeField] private InputField createRoomInput;
    [SerializeField] private InputField joinRoomInput;
    [SerializeField] private InputField playerNameInput;

    [Header("UI Elements")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject joinButtonPrefab;
    [SerializeField] private Transform roomListContainer;
    [SerializeField] private GameObject startGameButton;

    [Header("Canvasess")]
    [SerializeField] private GameObject RoomsListCanvas;
    [SerializeField] private GameObject CreateCanvas;

    [Header("Cameras")]
    [SerializeField] private List<Camera> cameras;
    #endregion

    #region === Variables ===
    public static string playerNameInLobby;
    private bool pendingCreateRoom = false;
    #endregion

    #region === Unity Events ===
    private void Start()
    {
        if (!PhotonNetwork.InLobby)
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    #endregion

    #region === Photon Callbacks ===
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected -> Joining Lobby");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");

        if (pendingCreateRoom)
        {
            CreateRoomNow();
            pendingCreateRoom = false;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> updatedRoomList)
    {
        RefreshRoomList(updatedRoomList);
    }

    public override void OnJoinedRoom()
    {
        HandleJoinedRoom();
    }
    #endregion

    #region === Room Creation / Join ===
    public void CreateRoom()
    {
        if (!PhotonNetwork.InLobby)
        {
            pendingCreateRoom = true;
            PhotonNetwork.JoinLobby();
            return;
        }

        CreateRoomNow();
    }

    private void CreateRoomNow()
    {
        if (string.IsNullOrEmpty(createRoomInput.text))
        {
            Debug.LogWarning("Room name empty.");
            return;
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        RoomOptions options = new RoomOptions
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 4
        };

        PhotonNetwork.CreateRoom(createRoomInput.text, options);
    }

    public void JoinRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (string.IsNullOrEmpty(joinRoomInput.text))
        {
            Debug.LogWarning("Join field empty.");
            return;
        }

        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }
    #endregion

    #region === UI Handling ===
    private void RefreshRoomList(List<RoomInfo> roomList)
    {
        foreach (Transform child in roomListContainer)
            Destroy(child.gameObject);

        foreach (RoomInfo room in roomList)
        {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
                continue;

            GameObject button = Instantiate(joinButtonPrefab, roomListContainer);
            button.name = room.Name;

            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null) text.text = room.Name;

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                PhotonNetwork.JoinRoom(room.Name);
            });
        }
    }

    private void HandleJoinedRoom()
    {
        SetPlayerName();
        ServerOnlinePlayers.addOnlinePlayer();

        bool isHost = PhotonNetwork.IsMasterClient;

        startGameButton.SetActive(isHost);
        if (isHost)
        {
            startGameButton.GetComponent<Button>().onClick.RemoveAllListeners();
            startGameButton.GetComponent<Button>().onClick.AddListener(StartGame);
        }

        RoomsListCanvas.SetActive(false);
        CreateCanvas.SetActive(false);

        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
        string skinName = PlayerPrefs.GetString("Skin");

        Hashtable props = new Hashtable()
        {
            { "SkinName", skinName }
        };  
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Debug.Log("My Skin Is: " + skinName);
    }
    #endregion

    #region === Player Name ===
    private void SetPlayerName()
    {
        if (string.IsNullOrEmpty(playerNameInput.text))
            playerNameInput.text = "Player" + Random.Range(1000, 9999);

        playerNameInLobby = playerNameInput.text;
        PhotonNetwork.LocalPlayer.NickName = playerNameInLobby;
    }
    #endregion

    #region === Start Game ===
    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("not master client");
            return;
        }

        StartCoroutine(DelayedStartGame());
    }

    
    // chatgpt code to move the host to game scene while delay
    private IEnumerator DelayedStartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.AutomaticallySyncScene = true;

        yield return new WaitForSeconds(1f);

        PhotonNetwork.LoadLevel("Game");

        yield return new WaitForSeconds(0.5f);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Game")
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    #endregion

    #region spawnPlayer
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        newPlayer.NickName = playerNameInLobby;
        Debug.Log("and my name is: " + newPlayer.NickName);
    }
    #endregion

    #region === Errors ===
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Create Room Failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Join Room Failed: " + message);
    }
    #endregion
}
