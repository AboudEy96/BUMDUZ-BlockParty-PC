using UnityEngine;
using Photon.Pun;

public class SmoothNetworkTransform : MonoBehaviourPun, IPunObservable
{
    Vector3 networkPosition;
    Quaternion networkRotation;

    public float lerpSpeed = 10f;
    public float teleportThreshold = 3f;

    private void Awake()
    {
        networkPosition = transform.position;
        networkRotation = transform.rotation;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            // Smooth movement
            float distance = Vector3.Distance(transform.position, networkPosition);
            if (distance > teleportThreshold)
            {
                transform.position = networkPosition;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * lerpSpeed);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * lerpSpeed);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}