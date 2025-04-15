using UnityEngine;

public class PlayerUser : MonoBehaviour
{
    public string name;
    public int id;
    [Header("Player Material Skin Material Object")]
    public Material skinMaterial;
    [Header("Player Prefab ")]
    public GameObject prefab;

    public void SetupPlayer(string name, int id, Material skinMaterial, GameObject prefab)
    {
        this.name = name;
        this.id = id;
        this.prefab = prefab;
        this.skinMaterial = skinMaterial;
        Debug.Log($"Player created: {name}, ID: {id}, Skin: {skinMaterial.name}");

    }
}