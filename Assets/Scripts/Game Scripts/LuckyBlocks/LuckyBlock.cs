using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = System.Random;

public class LuckyBlock : LuckyBlockManager
{
    
    // player 
    // box [ speed , jump ] 
    // give reward to player
    public override void onTouch(GameObject theLB, GameObject pl)

    {
        string lName = LayerMask.LayerToName(theLB.layer);
        Destroy(theLB);
        giveReward(pl);
    }

    public override void giveReward(GameObject pl) 
    {
        if (pl.CompareTag("Player") && rewards.Count != 0)
        {
            Random random = new Random();
            int ranNext = random.Next(0, rewards.Count-1);
            GameObject theReward = Instantiate(rewards[ranNext].gameObject);
            theReward.transform.SetParent(pl.transform);
            Debug.Log(rewards[ranNext].gameObject.name);
//            Invoke("removeReward", 10);
            StartCoroutine(RemoveRewardAfterDelay(pl, theReward));
        }
    }

    private IEnumerator RemoveRewardAfterDelay(GameObject pl, GameObject reward)
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds before removing the reward
        foreach (Transform child in pl.transform)
        {
            if (child.gameObject == reward.gameObject)
            {
                Destroy(child.gameObject);
                Debug.Log("Reward removed: " + reward.name);
                yield break; 
            }
        }
    }
}

/*public class LuckyBlockPurple : LuckyBlockManager {
    public override void onTouch(GameObject _gameObject)
    {
        if (_gameObject.CompareTag("Purple"))
        {
            Debug.Log("Purple Luck");
            Destroy(_gameObject);
        }
    }
}
public class LuckyBlockYellow : LuckyBlockManager
{
    public override void onTouch(GameObject _gameObject)
    {
        if (_gameObject.CompareTag("Yellow"))
        {
            Debug.Log("Yellow Luck");
            Destroy(_gameObject);
        }
    }
}

public class LuckyBlockSliver : LuckyBlock
{
    public override void onTouch(GameObject _gameObject)
    {
        if (_gameObject.CompareTag("Sliver"))
        {
            Debug.Log("Sliver Luck");
            Destroy(_gameObject);
        }
    }
}*/