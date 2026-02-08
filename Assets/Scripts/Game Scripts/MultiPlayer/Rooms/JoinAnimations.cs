using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class JoinAnimation : MonoBehaviourPunCallbacks
{
    [SerializeField] private Animator  _anim;
    [SerializeField] private TextMeshPro _name;
    IEnumerator Start()
    {
        while (photonView == null || photonView.Owner == null) yield return null;

        
        var player = photonView.Owner;
        object value = null;

        while (!player.CustomProperties.TryGetValue("SpawnIndex", out value))
            yield return null;

        int spawnIndex = value switch
        {
            int i => i,
            byte b => b,
            _ => -1
        };
        
             
            _name = GetComponentInChildren<TextMeshPro>();
            _anim = GetComponent<Animator>();
            
            _name.text = player.NickName;
            _anim.SetTrigger($"LobbyPlayer{spawnIndex}");
            
            Debug.Log($"Player {spawnIndex} spawned");
            Debug.Log($"Animation LobbyPlayer{spawnIndex}");
            Debug.Log($"Player {_name.text} spawned");
    }

}