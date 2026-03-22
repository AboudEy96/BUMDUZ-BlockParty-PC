using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using TMPro;

public class PlayerWinEvent : MonoBehaviourPunCallbacks
{
    public static PlayerWinEvent Instance;

    [Header("Alive Players UI")]
    public TextMeshProUGUI alivePlayersText;

    private bool gameEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable() => gameEnded = false;

    public override void OnJoinedRoom()
    {
        gameEnded = false;
        UpdateAlivePlayersText();
    }

    public override void OnLeftRoom() => gameEnded = false;

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps != null && changedProps.ContainsKey("isDead"))
        {
            UpdateAlivePlayersText();
        }

        if (!PhotonNetwork.IsMasterClient) return;
        if (gameEnded) return;

        if (changedProps != null && changedProps.ContainsKey("isDead"))
            CheckIfPlayerWin();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdateAlivePlayersText();

        if (!PhotonNetwork.IsMasterClient) return;
        if (gameEnded) return;

        CheckIfPlayerWin();
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        CheckIfPlayerWin();
    }

    public void CheckIfPlayerWin()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (gameEnded) return;
        if (!GameStateManager.IsPlaying()) return;

        int survivedPlayers = 0;
        Photon.Realtime.Player lastAlivePlayer = null;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player == null || player.IsInactive) continue;

            bool isDead = false;
            if (player.CustomProperties.TryGetValue("isDead", out var v) && v is bool b)
                isDead = b;

            if (!isDead)
            {
                survivedPlayers++;
                lastAlivePlayer = player;
            }
        }

        if (survivedPlayers == 1 && lastAlivePlayer != null)
        {
            gameEnded = true;
            WhoWon(lastAlivePlayer);
        }
    }

    private void UpdateAlivePlayersText()
    {
        if (alivePlayersText == null) return;

        int alive = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player == null || player.IsInactive) continue;

            bool isDead = false;
            if (player.CustomProperties.TryGetValue("isDead", out var v) && v is bool b)
                isDead = b;

            if (!isDead) alive++;
        }

        alivePlayersText.text = $"Alive: {alive}";
    }

    private void WhoWon(Photon.Realtime.Player winner)
    {
        photonView.RPC(nameof(PrintWinner), RpcTarget.All, winner.NickName);
        photonView.RPC(nameof(RemoteGameEnd), RpcTarget.All);
        photonView.RPC(nameof(LeaveRoom), RpcTarget.All);
    }

    [PunRPC]
    private void RemoteGameEnd() => GameStateManager.SetState(GameState.GameOver);

    [PunRPC]
    private void LeaveRoom() => StartCoroutine(LeaveRoomCoroutine());

    private System.Collections.IEnumerator LeaveRoomCoroutine()
    {
        yield return new WaitForSeconds(7f);
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    private void PrintWinner(string winnerName)
    {
        Debug.Log("Winner: " + winnerName);
    }
}