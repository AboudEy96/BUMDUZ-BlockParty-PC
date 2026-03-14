using System;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerMaterial : MonoBehaviour
{
    public List<Material> _skinMaterials = new List<Material>();
    public List<Material> _MUMDUZMaterials = new List<Material>();

    public static SyncPlayerMaterial instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<Material> getCurrentMaterial(string nameOfCurrentCharacter)
    {
        if (nameOfCurrentCharacter.Contains("MUMDUZ"))
            return _MUMDUZMaterials;

        // default  BUMDUZ materials
        return _skinMaterials;
    }

    public Material GetMaterialByName(string matName, string playerObjectName)
    {
        List<Material> materials = playerObjectName.Contains("MUMDUZ")
            ? _MUMDUZMaterials
            : _skinMaterials;

        foreach (var mat in materials)
        {
            if (mat.name == matName)
                return mat;
        }

        return null;
    }
}