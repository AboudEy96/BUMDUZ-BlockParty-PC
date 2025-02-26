using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapChanger : MonoBehaviour
{
    public GameObject[] maps;
    private BlocksDestroyer _blocksDestroyer;
    public Transform Scoreboard;
    private int currentMapIndex = 0;
    //public Transform mapText;
    void Start()
    {
        ActiveMap(currentMapIndex);
    }

    public void ActiveMap(int index)
    {
        if (index >= 0 && index < maps.Length)
        {
            maps[index].SetActive(true);
            foreach (GameObject otherMaps in maps)
            {
                if (otherMaps == maps[index]) continue;
                otherMaps.SetActive(false);
            }
            {
                
            }
            showMapImageAndName(getMapName());
        }
    }

    public void runNextMap()
    {
            currentMapIndex = (currentMapIndex + 1) % maps.Length; // 3 + 1 % 3 = 0  
            ActiveMap(currentMapIndex);
    }
    public String getMapName()
    {
        return maps[currentMapIndex].name;
    }

    public int getCurrentMapIndex()
    {
        return currentMapIndex;
    }

    public GameObject GetGameObject()
    {
        return maps[currentMapIndex].gameObject;
    }

    public void showMapImageAndName(String nextMapName)
    {
        foreach (Transform obj in Scoreboard)
        {
            if (obj.gameObject.CompareTag("MapScoreboard"))
            {
                obj.gameObject.SetActive(obj.gameObject.name.Equals(nextMapName));
            }
        }
        foreach (Transform obj in Scoreboard)
        {
            TMP_Text tmp = obj.GetComponent<TMP_Text>();
            if (tmp != null)
            {
                tmp.text = nextMapName;
            }
        }
    }
}
