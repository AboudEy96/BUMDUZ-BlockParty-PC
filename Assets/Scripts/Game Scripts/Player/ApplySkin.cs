using Photon.Pun;
using UnityEngine;

public class ApplySkin : MonoBehaviourPun
{
    [SerializeField] private SkinnedMeshRenderer _renderer;
    
    
    void Start()
    {
        _renderer =  GetComponentInChildren<SkinnedMeshRenderer>();
        if (photonView.Owner.CustomProperties.TryGetValue("SkinName", out var s))
        {
            var mat = SyncPlayerMaterial.instance.GetMaterialByName((string)s);
            _renderer.material = mat;
        }
    }
}