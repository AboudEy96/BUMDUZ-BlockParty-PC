using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapChanger : MonoBehaviour
{
    [Header("Maps")]
    public GameObject[] maps;

    [Header("UI")]
    public Transform scoreboard;

    private List<int> _availableMaps = new List<int>();
    private int _currentMapIndex = 0;

    private void Awake()
    {
        InitializePool();
    }

    public void ActivateMap(int index)
    {
        if (index < 0 || index >= maps.Length) return;

        _currentMapIndex = index;
        _availableMaps.Remove(index);

        if (_availableMaps.Count == 0)
            InitializePool();

        for (int i = 0; i < maps.Length; i++)
            maps[i].SetActive(i == _currentMapIndex);

        ShowScoreboard(GetCurrentMapName());
    }

    public int PickNextMapIndex()
    {
        if (_availableMaps.Count == 0)
            InitializePool();

        int randomIndex = UnityEngine.Random.Range(0, _availableMaps.Count);
        return _availableMaps[randomIndex];
    }

    public GameObject GetCurrentMap()      => maps[_currentMapIndex];
    public string GetCurrentMapName()      => maps[_currentMapIndex].name;
    public int GetCurrentMapIndex()        => _currentMapIndex;

    public List<Transform> GetCubesFromCurrentMap()
    {
        var cubes = new List<Transform>();
        int cubeLayer = LayerMask.NameToLayer("Cube");

        foreach (Transform child in GetCurrentMap().transform)
        {
            if (child.gameObject.layer == cubeLayer)
                cubes.Add(child);
        }

        return cubes;
    }

    private void InitializePool()
    {
        _availableMaps = new List<int>();
        for (int i = 0; i < maps.Length; i++)
            _availableMaps.Add(i);
    }

    private void ShowScoreboard(string mapName)
    {
        foreach (Transform obj in scoreboard)
        {
            if (obj.CompareTag("MapScoreboard"))
                obj.gameObject.SetActive(obj.name.Equals(mapName));

            TMP_Text tmp = obj.GetComponent<TMP_Text>();
            if (tmp != null)
                tmp.text = mapName;
        }
    }
}