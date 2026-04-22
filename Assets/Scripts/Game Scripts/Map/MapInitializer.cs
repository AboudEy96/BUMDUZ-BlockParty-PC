using Photon.Pun;
using UnityEngine;

public class MapInitializer : MonoBehaviourPun
{
    void Start()
    {
        FindObjectOfType<MapChanger>().SetSpawnedMap(gameObject);
    }
}