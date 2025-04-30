using UnityEngine;

public class Events : MonoBehaviour
{
    [Header("Map Transform to fix the colors.")]
    public Transform MAP;

    [Header("Player GameObject")] public Transform PLAYER;
    private void OnEnable()
    {
        EventManager.PlayerDeath += OnPlayerDeath;
        EventManager.PlayerHit += OnPlayerHit;
        EventManager.PlayerFall += OnPlayerFall;
        EventManager.PlayerRespawn += OnPlayerRespawn;
        EventManager.PlayerQuit += OnPlayerQuit;
        EventManager.PlayerWin += OnPlayerWin;
        EventManager.PlayerJoin += OnPlayerJoin;
        EventManager.ColorSetup += OnColorSetup;
    }

    private void OnDisable()
    {
        EventManager.PlayerDeath -= OnPlayerDeath;
        EventManager.PlayerHit -= OnPlayerHit;
        EventManager.PlayerFall -= OnPlayerFall;
        EventManager.PlayerRespawn -= OnPlayerRespawn;
        EventManager.PlayerQuit -= OnPlayerQuit;
        EventManager.PlayerWin -= OnPlayerWin;
        EventManager.PlayerJoin -= OnPlayerJoin;
        EventManager.ColorSetup -= OnColorSetup;
    }

    private void OnPlayerDeath(PlayerUser player)
    {
        Debug.Log("Player Dead");
    }

    private void OnPlayerHit(GameObject player) { }
    private void OnPlayerFall(GameObject player) { }
    private void OnPlayerRespawn(GameObject player) { }
    private void OnPlayerQuit(GameObject player) { }
    private void OnPlayerWin(GameObject player) { }
    private void OnPlayerJoin(GameObject player) { }

    private void OnColorSetup()
    {
        foreach (GameObject cube in MAP) 
        {
            
        }
    }
}