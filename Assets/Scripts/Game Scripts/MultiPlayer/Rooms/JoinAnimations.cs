using Photon.Pun;
using UnityEngine;

public class JoinAnimation : MonoBehaviourPunCallbacks
{
    [SerializeField] private Animator  anim;
    public override void OnJoinedRoom()
    {
        if (!photonView.IsMine)
            return;

        anim.SetTrigger("LobbyPlayer2");
    }
}