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
        
        while (photonView == null || photonView.Owner == null)
            yield return null;
        
        _renderer =  GetComponentInChildren<SkinnedMeshRenderer>();
     //   _nameInput =  GetComponentInChildren<TextMeshPro>();
        if (photonView.Owner.CustomProperties.TryGetValue("SkinName", out var s))
        {
            var mat = SyncPlayerMaterial.instance.GetMaterialByName((string)s);
            _renderer.material = mat;
            _nameInput.text = photonView.Owner.NickName;
        }
    }
}