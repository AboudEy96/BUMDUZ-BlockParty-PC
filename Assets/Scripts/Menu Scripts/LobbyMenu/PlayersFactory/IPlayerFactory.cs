using UnityEngine;

namespace Menu_Scripts.LobbyMenu.PlayersInRoom
{
    public interface IPlayerFactory
    {
        IPlayer CreatePlayer(int slotIndex,GameObject playerCharacter, Material playerSkin);
    }
}