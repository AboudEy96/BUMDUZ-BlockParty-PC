using System.Collections;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class LuckyBlock : LuckyBlockManager
{
    private PhotonView _pv;
    
    private void Awake()
    {
        _pv = GetComponent<PhotonView>();

    }
    public override void OnTouch(GameObject theLB, GameObject pl)
    {
        PhotonView playerView = pl.GetComponent<PhotonView>();
        if (playerView == null || !playerView.IsMine) return;

        _pv.RPC(nameof(RPC_TouchLuckyBlock), RpcTarget.MasterClient,
                playerView.ViewID);
    }

    [PunRPC]
    private void RPC_TouchLuckyBlock(int playerViewID)
    {
        if (rewards.Count == 0) return;

        int ranNext = Random.Range(0, rewards.Count);
        string rewardName = rewards[ranNext].name;

        _pv.RPC(nameof(RPC_ApplyRewardAndDestroy), RpcTarget.All,
                playerViewID, rewardName);
    }

    [PunRPC]
    private void RPC_ApplyRewardAndDestroy(int playerViewID, string rewardName)
    {
        if (loadParticleEffect != null)
            Instantiate(loadParticleEffect, transform.position, Quaternion.identity);

        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView != null)
        {
            foreach (var reward in rewards)
            {
                if (reward.name == rewardName)
                {
                    GameObject theReward = Instantiate(reward, playerView.transform.position, Quaternion.identity);
                    theReward.transform.SetParent(playerView.transform);
                    Destroy(theReward, 7f);
                    Debug.Log($"Reward given: {rewardName} to player {playerViewID}");
                    break;
                }
            }
        }

        PhotonNetwork.Destroy(gameObject);
    }

    public override void GiveReward(GameObject pl) { }
}