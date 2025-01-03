using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class LuckyBlockManager : MonoBehaviour
{
    public abstract void onTouch(GameObject _GM, GameObject _PL);

    public abstract void giveReward(GameObject pl);
    public List<GameObject> rewards;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("LuckyBlock"))
        {
       //     Debug.Log("Player touched a Lucky Block!");
            onTouch(gameObject, other.gameObject);
        }
    }
    public bool LuckyBlockLayer(GameObject lbObject, String layer)
    {
        return lbObject.layer == LayerMask.NameToLayer(layer);
    }
    
}
