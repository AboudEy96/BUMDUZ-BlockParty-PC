using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ApplySkin : MonoBehaviourPun
{
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private TextMeshPro _nameInput;
    
    IEnumerator Start()
    {
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();

        if (photonView == null)
        {
            ApplyLocalSkin();
            yield break;
        }

        while (photonView.Owner == null)
            yield return null;

        //multiplayer get status while creating room Skin => SkinName
        if (photonView.Owner.CustomProperties.TryGetValue("SkinName", out var s))
        {
            ApplySkinByName((string)s);
            _nameInput.text = photonView.Owner.NickName;
        }
        else
        {
            ApplyLocalSkin();
        }
    }

    private void ApplyLocalSkin()
    {
        string skinName = PlayerPrefs.GetString("Skin", "Colorful");
        ApplySkinByName(skinName);
    }

    private void ApplySkinByName(string skinName)
    {
        var mat = SyncPlayerMaterial.instance.GetMaterialByName(skinName, gameObject.name);
        if (mat != null)
        {
            _renderer.material = mat;
        }
        else
        {
            Debug.LogWarning($"Material not found: {skinName} for {gameObject.name}");
        }
    }
}