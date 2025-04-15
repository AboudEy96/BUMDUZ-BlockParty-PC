using UnityEngine;

public class Events : EventManager
{
    protected GameObject p;
    
    public override void OnPlayerDeath(PlayerUser player)
    {
        Debug.Log("Player Dead");
    }

    public override void OnPlayerHit(GameObject player)
    {
    }

    public override void OnPlayerFall(GameObject player)
    {
    }

    public override void OnPlayerRespawn(GameObject player)
    {
    }

    public override void OnPlayerQuit(GameObject player)
    {
    }

    public override void OnPlayerWin(GameObject player)
    {
    }

    public override void OnPlayerJoin(GameObject player)
    {
     
    }
}