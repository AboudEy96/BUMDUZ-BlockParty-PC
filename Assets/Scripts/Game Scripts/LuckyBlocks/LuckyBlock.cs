using System;
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

        switch (lName)
        {
            case "LuckyYellow":
                Destroy(theLB);
                giveReward(pl);
                break;
            case "LuckyPurple":
                Destroy(theLB);
                giveReward(pl);
                break;
            default:
                Destroy(theLB);
                Debug.Log("Unknown Luckyblock.. please send this message to developers");
                break;
        }
    }

    public override void giveReward(GameObject pl) 
    {
        if (pl.CompareTag("Player"))
        {
            Random random = new Random();
            int ranNext = random.Next(0, rewards.Count-1);
            Debug.Log(rewards[ranNext].gameObject.name);
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