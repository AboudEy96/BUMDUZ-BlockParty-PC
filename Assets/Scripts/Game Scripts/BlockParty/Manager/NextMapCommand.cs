using UnityEngine;
using Photon.Pun;

public class NextMapCommand : IGameCommand
{
    private readonly PhotonView _photonView;
    private readonly MapChanger _mapChanger;

    public NextMapCommand(PhotonView photonView, MapChanger mapChanger)
    {
        _photonView = photonView;
        _mapChanger = mapChanger;
    }

    public void Execute()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int nextIndex = _mapChanger.PickNextMapIndex();
        _photonView.RPC("SyncNextMap", RpcTarget.AllBuffered, nextIndex);
    }
}