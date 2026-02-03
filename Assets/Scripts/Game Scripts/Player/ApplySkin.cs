using Photon.Pun;
using UnityEngine;

public class ApplySkin : MonoBehaviourPun
{
    [SerializeField] private Renderer _renderer;
    
    
    void Start()
    {
        
        if (photonView.Owner.CustomProperties.TryGetValue("SkinName", out var s))
        {
            var mat = SyncPlayerMaterial.instance.GetMaterialByName((string)s);
            _renderer.material = mat;
        }
    }
}