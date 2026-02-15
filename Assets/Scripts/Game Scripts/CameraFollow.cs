using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private PhotonView photonView;
    private Transform target;
    private Player targetPlayer;

    [Header("Base Camera Settings")]
    public Vector3 offset = new Vector3(0, 5, -10);
    public float rotationSpeed = 100f;
    public float followLerpSpeed = 10f;

    [Header("Run Camera Effects")]
    public float runBackwardDistance = 2f;
    public float runOffsetLerpSpeed = 6f;
    public float shakeAmount = 0.08f;
    public float shakeFrequency = 18f;

    private float currentYaw = 0f;
    private float currentRunOffset;

    private void Start()
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
        targetPlayer = target?.GetComponent<Player>();

        if (target == null || photonView == null || !photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (target == null || photonView == null || !photonView.IsMine) return;

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        currentYaw += mouseX;

        bool isRunning = targetPlayer != null && targetPlayer.IsRunning;
        float targetRunOffset = isRunning ? runBackwardDistance : 0f;
        currentRunOffset = Mathf.Lerp(currentRunOffset, targetRunOffset, runOffsetLerpSpeed * Time.deltaTime);

        Vector3 dynamicOffset = offset + new Vector3(0f, 0f, -currentRunOffset);
        Vector3 desiredPosition = target.position + Quaternion.Euler(0, currentYaw, 0) * dynamicOffset;

        if (isRunning)
        {
            float shakeX = (Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) - 0.5f) * 2f * shakeAmount;
            float shakeY = (Mathf.PerlinNoise(0f, Time.time * shakeFrequency) - 0.5f) * 2f * shakeAmount;
            desiredPosition += transform.right * shakeX + transform.up * shakeY;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followLerpSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}
