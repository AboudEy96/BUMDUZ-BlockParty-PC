using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class LuckyBlockManager : MonoBehaviourPun
{
    [Header("Lucky Block Destroy Effect")]
    public GameObject loadParticleEffect;

    public List<GameObject> rewards;
    public List<GameObject> luckyBlocks;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!gameObject.CompareTag("LuckyBlock")) return;

        OnTouch(gameObject, other.gameObject);
    }

    public virtual void OnTouch(GameObject lb, GameObject player) { }
    public virtual void GiveReward(GameObject player) { }
}