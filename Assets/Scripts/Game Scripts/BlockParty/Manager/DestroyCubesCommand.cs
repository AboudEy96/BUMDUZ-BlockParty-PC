using UnityEngine;
using Photon.Pun;

public class DestroyCubesCommand : IGameCommand
{
    private readonly PhotonView _photonView;
    private readonly string _chosenTag;

    public DestroyCubesCommand(PhotonView photonView, string chosenTag)
    {
        _photonView = photonView;
        _chosenTag = chosenTag;
    }

    public void Execute()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _photonView.RPC("SyncDestroyCubes", RpcTarget.AllBuffered, _chosenTag);
    }
}
