using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class MapChanger : MonoBehaviour
{
    [Header("Maps")]
    public GameObject[] maps;
    public GameObject mapsFather;
    
    [Header("UI")]
    public Transform scoreboard;

    private List<int> _availableMaps = new List<int>();
    private int _currentMapIndex = 0;
    private GameObject _spawnedMapInstance;

    private void Awake()
    {
        InitializePool();
    }

    public void ActivateMap(int index)
    {
        if (index < 0 || index >= maps.Length) return;

        if (_spawnedMapInstance != null && _spawnedMapInstance.GetComponent<PhotonView>().IsMine)
            PhotonNetwork.Destroy(_spawnedMapInstance);

        _currentMapIndex = index;
        _availableMaps.Remove(index);

        if (_availableMaps.Count == 0)
            InitializePool();

        if (PhotonNetwork.IsMasterClient)
        {
            _spawnedMapInstance = PhotonNetwork.Instantiate(
                $"MapsPrefabs/{maps[_currentMapIndex].name}",
                mapsFather.transform.position,
                mapsFather.transform.rotation
            );
        }

        ShowScoreboard(GetCurrentMapName());
    }

    public void SetSpawnedMap(GameObject map)
    {
        _spawnedMapInstance = map;
    }

    public int PickNextMapIndex()
    {
        if (_availableMaps.Count == 0)
            InitializePool();

        int randomIndex = UnityEngine.Random.Range(0, _availableMaps.Count);
        return _availableMaps[randomIndex];
    }

    public GameObject GetCurrentMap()      => _spawnedMapInstance;
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
            switch (obj.gameObject.name)
            {
                case "MAP_NAME":
                    if (tmp != null)
                        tmp.text = mapName;
                    break;
                case "ROUND":
                    if (tmp != null)
                        tmp.text = $"({RoundRewardManager.Instance.GetRound()})";
                    break;
                case "SCORE":
                    if (tmp != null)
                        tmp.text = $"({PlayerDataManager.Instance.GetWins()})";      
                    break;
                case "BALANCE":
                    if (tmp != null)
                        tmp.text = $"({PlayerDataManager.Instance.GetCoins()})";
                    break;
            }
       
        }
    }
}