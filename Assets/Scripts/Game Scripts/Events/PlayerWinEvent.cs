using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerWinEvent : MonoBehaviourPunCallbacks
{
    public static PlayerWinEvent Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    public void OnPlayerWin(Photon.Realtime.Player pl)
    {
        GameStartSingletoon.GetInstance().GameEnd();
    }

    public void CheckIfPlayerWin()
    {
        int SurvivedPlayers = 0;
        Photon.Realtime.Player lastAlivePlayer = null;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            bool isDead = false;
            if (player.CustomProperties.ContainsKey("isDead"))
                isDead = (bool)player.CustomProperties["isDead"];

            if (!isDead)
            {
                SurvivedPlayers++;
                lastAlivePlayer = player;
            }
        }
        
        if (SurvivedPlayers == 1)
        {
            WhoWon(lastAlivePlayer);
        }
        Debug.Log($"[CheckIfPlayerWin] SurvivedPlayers={SurvivedPlayers} lastAlive={(lastAlivePlayer != null ? lastAlivePlayer.NickName : "null")}");

    }


    public void WhoWon(Photon.Realtime.Player winner)
    {
        photonView.RPC("PrintWinner", RpcTarget.All, winner.NickName);
        photonView.RPC("LeaveRoom", RpcTarget.All);
        OnPlayerWin(winner);
    }

    [PunRPC]
    void LeaveRoom()
    {
        StartCoroutine(LeaveRoomCoroutine());
    }

    private IEnumerator LeaveRoomCoroutine()
    {
        yield return new WaitForSeconds(7f);

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    [PunRPC]
    void PrintWinner(string winnerName)
    {
        Debug.Log("Winner of the game is: " + winnerName);
    }
}