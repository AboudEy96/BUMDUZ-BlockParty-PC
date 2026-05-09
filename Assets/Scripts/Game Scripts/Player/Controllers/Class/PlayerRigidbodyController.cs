using UnityEngine;

public class PlayerRigidbodyController : MonoBehaviour, IActivateable
{
    void OnEnable()
    {
        MoveController.Register(this);
    }

    void OnDisable()
    {
        MoveController.Unregister(this);
    }
    public void SetActive(bool state)
    {
        GetComponent<Rigidbody>().isKinematic = !state;
    }
}