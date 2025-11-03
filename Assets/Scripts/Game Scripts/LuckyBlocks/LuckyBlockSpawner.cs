using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class LuckyBlockSpawner : LuckyBlockManager
{
    public int height;
    public int width;
   // public GameObject _GM;
    public int yAXIS;
    
    private ParticleSystem fireParticles;

    void Start()
    {
        Invoke("SpawnLuckyBlock", 10f);
    }

    public void SpawnLuckyBlock()
    {
        if (GameStartSingletoon.GetInstance().isGameStarted)
        {
            int x = Random.Range(-19, width);
            int z = Random.Range(-21, height);
            int ranNext = Random.Range(0, luckyBlocks.Count);
            Vector3 location = new Vector3(x, yAXIS, z);
            GameObject theReward = Instantiate(luckyBlocks[ranNext].gameObject, location, Quaternion.identity);

            Console.WriteLine($"Luckyblock Spanwed {theReward.transform.name}");
            Invoke("SpawnLuckyBlock", 25f);
        }
    }
}
