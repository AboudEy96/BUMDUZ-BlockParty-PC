using UnityEngine;

namespace Menu_Scripts.LobbyMenu.PlayersInRoom
{
    
    public interface IPlayer
    {
        
        void Initialize(int actorNumber,GameObject playerCharacter, Material playerSkin);

    }
}