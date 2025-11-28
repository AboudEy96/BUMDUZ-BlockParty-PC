using System;
using UnityEngine;

public abstract class EventManager : MonoBehaviour
{
    public static event Action<PlayerUser> PlayerDeath;
    public static event Action<GameObject> PlayerHit;
    public static event Action<GameObject> PlayerFall;
    public static event Action<GameObject> PlayerRespawn;
    public static event Action<GameObject> PlayerQuit;
    public static event Action<GameObject> PlayerWin;
    public static event Action<GameObject> PlayerJoin;
    public static event Action ColorSetup;
        
    public static void TriggerPlayerDeath(PlayerUser player) => PlayerDeath?.Invoke(player);
    public static void TriggerPlayerHit(GameObject player) => PlayerHit?.Invoke(player);
    public static void TriggerPlayerFall(GameObject player) => PlayerFall?.Invoke(player);
    public static void TriggerPlayerRespawn(GameObject player) => PlayerRespawn?.Invoke(player);
    public static void TriggerPlayerQuit(GameObject player) => PlayerQuit?.Invoke(player);
    public static void TriggerPlayerWin(GameObject player) => PlayerWin?.Invoke(player);
    public static void TriggerPlayerJoin(GameObject player) => PlayerJoin?.Invoke(player);
    public static void TriggerColorSetup() => ColorSetup?.Invoke();
    
}