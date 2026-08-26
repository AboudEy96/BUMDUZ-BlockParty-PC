using Photon.Pun;
using UnityEngine;

public class ActivePlayerCount : MonoBehaviour
{
    public static ActivePlayerCount instance;
    
    private int aLifePlayers = PhotonNetwork.PlayerList.Length;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
   //     DontDestroyOnLoad(gameObject);
    }
    public int GetActivePlayersCount()
    {
        return aLifePlayers;
    }
    public void SetActivePlayersCount(int count)
    {
        aLifePlayers = count;
    }
    public void RemoveActivePlayer()
    {
        aLifePlayers--;
    }
    public static ActivePlayerCount GetInstance()
    {
        return instance;
    }
    
    
}