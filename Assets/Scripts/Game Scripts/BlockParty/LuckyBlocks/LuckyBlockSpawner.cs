using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class LuckyBlockSpawner : LuckyBlockManager
{
    public int height;
    public int width;
    public int yAXIS;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            Invoke(nameof(SpawnLuckyBlock), 10f);
    }

    public void SpawnLuckyBlock()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!GameStateManager.IsPlaying()) return;

        int x = Random.Range(-19, width);
        int z = Random.Range(-21, height);
        int ranNext = Random.Range(0, luckyBlocks.Count);
        Vector3 location = new Vector3(x, yAXIS, z);
        GameObject spawnedLucky = PhotonNetwork.Instantiate(
            luckyBlocks[ranNext].name,
            location,
            Quaternion.identity
        );
        Debug.Log($"LuckyBlock spawned: {luckyBlocks[ranNext].name}");
        Invoke(nameof(SpawnLuckyBlock), 25f);
    }
}