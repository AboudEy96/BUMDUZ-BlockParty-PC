using Menu_Scripts.LobbyMenu.PlayersInRoom;
using UnityEngine;

public class ShowPlayerSkin : MonoBehaviour, IPlayer
{
    
    public Renderer playerRenderer;

    public void Initialize(int actorNum, SkinInfo playerSkin)
    {
        if(playerRenderer != null && playerSkin.material != null)
            playerRenderer.material = playerSkin.material;

        Debug.Log($"Player {actorNum} initialized with skin {playerSkin.skinName}");

    }
}