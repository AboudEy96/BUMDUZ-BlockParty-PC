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
    public List<int> availableMaps; // لتخزين الماب المتاحة
    private int currentMapIndex = 0;

    void Start()
    {
        InitializeMaps();
        ActiveMap(currentMapIndex);
    }

    // تهيئة الخرائط المتاحة
    private void InitializeMaps()
    {
        availableMaps = new List<int>();
        for (int i = 0; i < maps.Length; i++)
        {
            availableMaps.Add(i); // إضافة جميع الخرائط إلى القائمة
        }
    }

    // تفعيل الخريطة بناءً على الفهرس
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

    // تشغيل الخريطة التالية بشكل عشوائي
    public void runNextMap()
    {
        if (availableMaps.Count > 0)
        {
            // اختيار ماب عشوائي من الخرائط المتاحة
            int randomIndex = UnityEngine.Random.Range(0, availableMaps.Count);
            currentMapIndex = availableMaps[randomIndex];
            availableMaps.RemoveAt(randomIndex); // إزالة الخريطة المختارة من القائمة

            ActiveMap(currentMapIndex);
        }
        else
        {
            // إذا انتهت الخرائط المتاحة، إعادة تفعيل كل الخرائط
            InitializeMaps();
            // اختيار ماب عشوائي من الخرائط المتاحة
            int randomIndex = UnityEngine.Random.Range(0, availableMaps.Count);
            currentMapIndex = availableMaps[randomIndex];
            availableMaps.RemoveAt(randomIndex); // إزالة الخريطة المختارة من القائمة

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
