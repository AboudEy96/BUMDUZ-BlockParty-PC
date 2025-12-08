using Menu_Scripts.LobbyMenu.PlayersInRoom;
using UnityEngine;

public class ShowPlayerSkin : MonoBehaviour, IPlayer
{
    
    public Renderer playerRenderer;

    public void Initialize(int actorNum, GameObject playerCharacter, Material playerSkin)
    {
        if(playerRenderer != null && playerSkin != null)
            playerRenderer.material = playerSkin;

        Debug.Log($"player {actorNum} initialized with skin {playerSkin.name}");

    }
}