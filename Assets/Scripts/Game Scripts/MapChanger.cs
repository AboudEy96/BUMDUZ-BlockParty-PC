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
    public List<int> availableMaps;
    private int currentMapIndex = 0;

    void Start()
    {
            InitializeMaps();
            currentMapIndex = UnityEngine.Random.Range(0, availableMaps.Count);
            ActiveMap(currentMapIndex);
            GameObject map = maps[currentMapIndex];
            ColorChangeEvent.SetUpColors(map.transform);
            availableMaps.RemoveAt(currentMapIndex);
    }

    private void InitializeMaps()
    {
        availableMaps = new List<int>();
        for (int i = 0; i < maps.Length; i++)
        {
            availableMaps.Add(i);
        }
    }


    public void ActiveMap(int index)
    {
        if (index >= 0 && index < maps.Length)
        {
            maps[index].SetActive(true);
            foreach (GameObject otherMap in maps)
            {
                if (otherMap == maps[index]) continue;
                otherMap.SetActive(false);
            }
            showMapImageAndName(getMapName());
        }
    }


    public void runNextMap()
    {
        if (availableMaps.Count > 0)
        {

            int randomIndex = UnityEngine.Random.Range(0, availableMaps.Count);
            currentMapIndex = availableMaps[randomIndex];
            availableMaps.RemoveAt(randomIndex); 

            ActiveMap(currentMapIndex);
        }
        else
        {
            InitializeMaps();
            int randomIndex = UnityEngine.Random.Range(0, availableMaps.Count);
            currentMapIndex = availableMaps[randomIndex];
            availableMaps.RemoveAt(randomIndex);
            ActiveMap(currentMapIndex);
        }
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
