using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class LuckyBlockManager : MonoBehaviour
{
    public virtual void OnTouch(GameObject _GM, GameObject _PL){}
    public virtual void GiveReward(GameObject pl){}
  //  public abstract void removeReward(GameObject pl, GameObject reward);
    public List<GameObject> rewards;
    public List<GameObject> luckyBlocks;

    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("LuckyBlock"))
        {
            //     Debug.Log("Player touched a Lucky Block!");
            OnTouch(gameObject, other.gameObject);
        }
    }

    public bool LuckyBlockLayer(GameObject lbObject, String layer)
    {
        return lbObject.layer == LayerMask.NameToLayer(layer);
    }
    
}
