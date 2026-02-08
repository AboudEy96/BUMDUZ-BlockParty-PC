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
    [SerializeField] private PhotonPlayerFactory playerFactory;

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

    [Header("In Room and In Lobby")]
    [SerializeField] private GameObject OUT_LOBBY;
    [SerializeField] private GameObject IN_LOBBY;

    public static string playerNameInLobby;
    private bool pendingCreateRoom = false;

    private void Start()
    {
        
        if (!PhotonNetwork.IsConnected){
            PhotonNetwork.ConnectUsingSettings();
        return; }
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby(TypedLobby.Default);

    }

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        if (pendingCreateRoom)
        {
            CreateRoomNow();
            pendingCreateRoom = false;
        }
    }

    #region Rooms Show Update

    private readonly Dictionary<string, RoomInfo> cachedRooms = new Dictionary<string, RoomInfo>();

    public override void OnRoomListUpdate(List<RoomInfo> updatedRoomList)
    {
        if (updatedRoomList == null || updatedRoomList.Count == 0)
            return;

        RefreshRoomList(updatedRoomList);
    }

    private void RefreshRoomListFromCache()
    {
        foreach (Transform child in roomListContainer)
            Destroy(child.gameObject);

        foreach (var kvp in cachedRooms)
        {
            RoomInfo room = kvp.Value;

            if (!room.IsOpen || room.MaxPlayers > 0 && room.PlayerCount >= room.MaxPlayers)
                continue;

            GameObject button = Instantiate(joinButtonPrefab, roomListContainer);
            button.name = room.Name;

            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null) text.text = $"{room.Name} ({room.PlayerCount}/{room.MaxPlayers})";

            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() => PhotonNetwork.JoinRoom(room.Name));
        }

    }
    #endregion

    public override void OnJoinedRoom()
    {
        HandleJoinedRoom();
        ChangeScreen();
    }

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
            return;

        PhotonNetwork.AutomaticallySyncScene = true;

        RoomOptions options = new RoomOptions
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 4
        };

        PhotonNetwork.CreateRoom(createRoomInput.text, options);
     //   ChangeScreen();
    }

    public void ChangeScreen()
    {
        OUT_LOBBY.SetActive(!OUT_LOBBY.activeSelf);
        IN_LOBBY.SetActive(!IN_LOBBY.activeSelf);
        cameras[0].gameObject.SetActive(!cameras[0].gameObject.activeSelf);
        //cameras[1].gameObject.SetActive(!cameras[1].gameObject.activeSelf);
    }

    public void JoinRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (string.IsNullOrEmpty(joinRoomInput.text))
            return;

        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

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

        bool isHost = PhotonNetwork.IsMasterClient;
        startGameButton.SetActive(isHost);

        if (isHost)
        {
            startGameButton.GetComponent<Button>().onClick.RemoveAllListeners();
            startGameButton.GetComponent<Button>().onClick.AddListener(StartGame);
        }

        RoomsListCanvas.SetActive(false);
        CreateCanvas.SetActive(false);

        string skinName = PlayerPrefs.GetString("Skin");
        int spawnIndex = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % 4;

        Hashtable props = new Hashtable
        {
            { "SkinName", skinName },
            {"SpawnIndex", spawnIndex}
        };
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        Material playerMaterial = SyncPlayerMaterial.instance.GetMaterialByName(skinName);

        playerFactory.CreatePlayer(
            spawnIndex,
            PlayerCharacterSingletoon.instance.LOBBY_CHARACTER,
            playerMaterial
        );
    }
    

    private void SetPlayerName()
    {
        if (string.IsNullOrEmpty(playerNameInput.text))
            playerNameInput.text = "Player" + Random.Range(1000, 9999);

        playerNameInLobby = playerNameInput.text;
        PhotonNetwork.LocalPlayer.NickName = playerNameInLobby;
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        StartCoroutine(DelayedStartGame());
    }

    private IEnumerator DelayedStartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.AutomaticallySyncScene = true;

        yield return new WaitForSeconds(1f);

        PhotonNetwork.LoadLevel("Game");
        
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Game")
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            //     PhotonNetwork.LoadLevel("GameScene1,2");
            PhotonNetwork.LoadLevel("Game");
            Debug.LogWarning("Photon load canceled, forcing local scene load...");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError(message);
    }
}
