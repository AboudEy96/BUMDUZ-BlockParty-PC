using System;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject player;
    public int height;
    public int width;
    public int yAXIS;

     void Start()
    {
        int x = Random.Range(-19, width);
        int z = Random.Range(-21, height);
        Vector3 location = new Vector3(x, yAXIS, z);
        PhotonNetwork.Instantiate(player.name, location, Quaternion.identity);
    }
}
