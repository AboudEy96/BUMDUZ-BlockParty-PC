using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class SelectColorCommand : IGameCommand
{
    private readonly PhotonView _photonView;
    private readonly MapChanger _mapChanger;

    public SelectColorCommand(PhotonView photonView, MapChanger mapChanger)
    {
        _photonView = photonView;
        _mapChanger = mapChanger;
    }

    public void Execute()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        List<Transform> cubes = _mapChanger.GetCubesFromCurrentMap();
        string selectedTag = PickRandomTag(cubes);

        _photonView.RPC("SyncSelectedColor", RpcTarget.AllBuffered, selectedTag);
    }

    private string PickRandomTag(List<Transform> cubes)
    {
        HashSet<string> tags = new HashSet<string>();
        foreach (Transform cube in cubes)
            tags.Add(cube.tag);

        string[] tagArray = tags.ToArray();
        return tagArray[Random.Range(0, tagArray.Length)];
    }
}