/*using UnityEngine;
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
            stream.SendNext(transform.position.x);
            stream.SendNext(transform.position.y);
            stream.SendNext(transform.position.z);

            stream.SendNext(transform.rotation.x);
            stream.SendNext(transform.rotation.y);
            stream.SendNext(transform.rotation.z);
            stream.SendNext(transform.rotation.w);
        }
        else
        {
            float px = (float)stream.ReceiveNext();
            float py = (float)stream.ReceiveNext();
            float pz = (float)stream.ReceiveNext();
            networkPosition = new Vector3(px, py, pz);

            float rx = (float)stream.ReceiveNext();
            float ry = (float)stream.ReceiveNext();
            float rz = (float)stream.ReceiveNext();
            float rw = (float)stream.ReceiveNext();
            networkRotation = new Quaternion(rx, ry, rz, rw);
        }
    }
}*/