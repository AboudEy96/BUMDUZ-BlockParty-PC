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

    [Header("Winner Canvas")]
    public Canvas winnerCanvas;
    public TextMeshProUGUI winnerNameText;
    public TextMeshProUGUI winnerRoundsText;

    private bool gameEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (winnerCanvas != null)
            winnerCanvas.gameObject.SetActive(false);
    }

    public override void OnEnable()
    {
        base.OnEnable(); 
        gameEnded = false;
    }

    public override void OnDisable()
    {
        base.OnDisable(); 
    }

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

        int totalPlayers = PhotonNetwork.PlayerList.Length;
        int deadPlayers = totalPlayers - survivedPlayers;
        if (deadPlayers == 0) return; 

        if (survivedPlayers == 1 && lastAlivePlayer != null)
        {
            gameEnded = true;
            WhoWon(lastAlivePlayer);
        }

        if (survivedPlayers == 0)
        {
            gameEnded = true;
            photonView.RPC(nameof(RPC_ShowDraw), RpcTarget.All);
            photonView.RPC(nameof(RemoteGameEnd), RpcTarget.All);
            photonView.RPC(nameof(LeaveRoom), RpcTarget.All);
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

        alivePlayersText.text = $"Players Alive: <color=green>({alive})</color>";
    }

    private void WhoWon(Photon.Realtime.Player winner)
    {
        int rounds = GameRoundManager.Score;

        photonView.RPC(nameof(RPC_ShowWinner), RpcTarget.All, winner.NickName, winner.ActorNumber, rounds);
        photonView.RPC(nameof(RPC_RewardWinner), RpcTarget.All, winner.ActorNumber);
        photonView.RPC(nameof(RemoteGameEnd), RpcTarget.All);
        photonView.RPC(nameof(LeaveRoom), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_ShowWinner(string winnerName, int winnerActorNumber, int rounds)
    {
        Debug.Log("Winner: " + winnerName + " | Rounds: " + rounds);

        if (winnerCanvas != null)
            winnerCanvas.gameObject.SetActive(true);

        if (winnerNameText != null)
            winnerNameText.text = winnerName;

        if (winnerRoundsText != null)
            winnerRoundsText.text = $"Rounds Won: {rounds}";
    }

    [PunRPC]
    private void RPC_ShowDraw()
    {
        Debug.Log("Draw - all players died!");

        if (winnerCanvas != null)
            winnerCanvas.gameObject.SetActive(true);

        if (winnerNameText != null)
            winnerNameText.text = "Draw!";

        if (winnerRoundsText != null)
            winnerRoundsText.text = "";
    }

    [PunRPC]
    private void RPC_RewardWinner(int winnerActorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == winnerActorNumber)
            RoundRewardManager.Instance?.OnGameWon();
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
}