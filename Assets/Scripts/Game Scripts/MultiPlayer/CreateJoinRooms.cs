using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private readonly Dictionary<string, RoomInfo> cachedRooms = new Dictionary<string, RoomInfo>();
    private readonly Dictionary<string, GameObject> roomButtons = new Dictionary<string, GameObject>();

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        StartCoroutine(JoinLobbyAfterUIReady());
    }
    private IEnumerator JoinLobbyAfterUIReady()
    {
        yield return null;

        if (PhotonNetwork.InLobby)
            PhotonNetwork.LeaveLobby();

        while (PhotonNetwork.InLobby)
            yield return null;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            SceneManager.LoadScene("Lobby");
        }
    }

    public override void OnJoinedLobby()
    {
        if (pendingCreateRoom)
        {
            CreateRoomNow();
            pendingCreateRoom = false;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> updatedRoomList)
    {
        if (updatedRoomList == null) return;

        foreach (var info in updatedRoomList)
        {
            if (info.RemovedFromList || !info.IsVisible || !info.IsOpen)
            {
                cachedRooms.Remove(info.Name);

                if (roomButtons.TryGetValue(info.Name, out var go) && go != null)
                    Destroy(go);

                roomButtons.Remove(info.Name);
                continue;
            }

            cachedRooms[info.Name] = info;

            if (!roomButtons.TryGetValue(info.Name, out var buttonGO) || buttonGO == null)
            {
                buttonGO = Instantiate(joinButtonPrefab, roomListContainer);
                roomButtons[info.Name] = buttonGO;

                var btn = buttonGO.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                string roomNameCopy = info.Name;
                btn.onClick.AddListener(() => PhotonNetwork.JoinRoom(roomNameCopy));
            }

            var roomNameTransform = buttonGO.transform.Find("ROOM NAME");
            if (roomNameTransform != null)
            {
                var roomNameText = roomNameTransform.GetComponent<TextMeshProUGUI>();
                if (roomNameText != null)
                    roomNameText.text = $"{info.Name} ({info.PlayerCount}/{info.MaxPlayers})";
            }

            bool full = info.MaxPlayers > 0 && info.PlayerCount >= info.MaxPlayers;
            buttonGO.SetActive(!full);
        }
    }


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
            PhotonNetwork.JoinLobby(TypedLobby.Default);
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

        PhotonNetwork.CreateRoom(createRoomInput.text, options, TypedLobby.Default);
    }

    public void ChangeScreen()
    {
        OUT_LOBBY.SetActive(!OUT_LOBBY.activeSelf);
        IN_LOBBY.SetActive(!IN_LOBBY.activeSelf);
        RoomsListCanvas.SetActive(!RoomsListCanvas.activeSelf);
        CreateCanvas.SetActive(!CreateCanvas.activeSelf);
        cameras[0].gameObject.SetActive(!cameras[0].gameObject.activeSelf);
    }

    public void JoinRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (string.IsNullOrEmpty(joinRoomInput.text))
            return;

        PhotonNetwork.JoinRoom(joinRoomInput.text);
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

        string skinName = PlayerPrefs.GetString("Skin");
        int spawnIndex = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % 4;

        Hashtable props = new Hashtable
        {
            { "SkinName", skinName },
            { "SpawnIndex", spawnIndex }
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

        if (SceneManager.GetActiveScene().name != "Game")
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("Game");
            Debug.LogWarning("Photon load canceled, forcing local scene load...");
            SceneManager.LoadScene("Game");
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        startGameButton.SetActive(false);
        ChangeScreen();
        Debug.Log("Leave Room");
    }

    public void LeaveLobby()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("MainScene");
        PhotonNetwork.OfflineMode = true;
        Debug.Log("Leave Lobby");
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
