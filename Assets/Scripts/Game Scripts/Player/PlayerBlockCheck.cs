using System;
using UnityEngine;

public class PlayerBlockCheck : MonoBehaviour
{
    public Transform lights;
    private void OnTriggerStay(Collider other)
    {
            if (BlocksDestroyer.chosenTag == null)
            {
                ShowStayBlockStatus(lights, "idle");
            }
            else
            {
                bool isOnBlock = other.transform.CompareTag(BlocksDestroyer.chosenTag);
                ShowStayBlockStatus(lights, isOnBlock ? "on" : "off");
            }
    }
    private void ShowStayBlockStatus(Transform image, string name) 
    {
        foreach (Transform l in lights)
        {
            
            if (l.transform.CompareTag("LightON"))
            {
                l.gameObject.SetActive(l.transform.name.Equals(name));
            }
        }
    }
}
