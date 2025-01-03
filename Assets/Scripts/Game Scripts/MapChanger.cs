using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChanger : MonoBehaviour
{
    public GameObject[] maps;
    private BlocksDestroyer _blocksDestroyer;
    private int currentMapIndex = 0;
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
}
