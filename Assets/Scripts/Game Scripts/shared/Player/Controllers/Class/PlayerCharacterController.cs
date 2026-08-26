using Photon.Pun;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour, IActivateable
{
    private CharacterController cc;
    private PhotonView view;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        view = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        MoveController.Register(this);
    }

    private void OnDisable()
    {
        MoveController.Unregister(this);
    }

    public void SetActive(bool state)
    {
        if (!view.IsMine) return;

        cc.enabled = state;
    }
}