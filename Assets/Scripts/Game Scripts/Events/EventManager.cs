using UnityEngine;

public abstract class EventManager : MonoBehaviour
{
    public virtual void OnPlayerDeath(GameObject player)
    {
        
    }
    public virtual void OnPlayerHit(GameObject player){}
    public virtual void OnPlayerFall(GameObject player){}
    public virtual void OnPlayerRespawn(GameObject player){}
    public virtual void OnPlayerQuit(GameObject player){}
    public virtual void OnPlayerWin(GameObject player){}
    public virtual void OnPlayerJoin(GameObject player){}
    
}