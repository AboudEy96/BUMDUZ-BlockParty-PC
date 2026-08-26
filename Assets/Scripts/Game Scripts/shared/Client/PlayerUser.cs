using TMPro;
using UnityEngine;

public class PlayerUser : MonoBehaviour
{
    public string name;
    public int id;
    [Header("Player Material Skin Material Object")]
    public string skinMaterial;
    [Header("Player Prefab ")]
    public GameObject prefab;
    [SerializeField] private TextMeshPro _nameInput;

    public void SetupPlayer(string name, int id, string skinMaterial, GameObject prefab)
    {
        this.name = name;
        this.id = id;
        this.prefab = prefab;
        this.skinMaterial = skinMaterial;
        this._nameInput.text = name;
        Debug.Log($"Player created: {name}, ID: {id}, Skin: {skinMaterial}");

    }
}