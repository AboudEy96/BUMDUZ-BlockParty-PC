using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private PhotonView photonView;
    private Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float rotationSpeed = 100f;
    private float currentYaw = 0f;

    void Start()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                target = player.transform;
                break;
            }
        }
        photonView = target?.GetComponent<PhotonView>();
        if (target == null || !photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (target == null || !photonView.IsMine) return;

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        currentYaw += mouseX;

        Vector3 desiredPosition = target.position + Quaternion.Euler(0, currentYaw, 0) * offset;
        transform.position = desiredPosition;
        transform.LookAt(target);
    }
}