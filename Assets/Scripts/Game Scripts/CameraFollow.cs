using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private PhotonView photonView;
    private Transform target;
    private Player targetPlayer;

    public Vector3 offset = new Vector3(0, 5, -10);
    public float rotationSpeed = 100f;
    public float followLerpSpeed = 10f;

    public float runExtraDistance = 3f;
    public float runLerpSpeed = 6f;

    private float currentYaw = 0f;
    private float currentExtraDistance = 0f;

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
            gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (target == null || photonView == null || !photonView.IsMine) return;

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        currentYaw += mouseX;

        bool isRunning = targetPlayer != null && targetPlayer.IsRunning;

        float targetExtra = isRunning ? runExtraDistance : 0f;
        currentExtraDistance = Mathf.Lerp(currentExtraDistance, targetExtra, runLerpSpeed * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(0f, currentYaw, 0f);

        Vector3 direction = (rotation * offset).normalized;

        float baseDistance = offset.magnitude;

        float finalDistance = baseDistance + currentExtraDistance;

        Vector3 desiredPosition = target.position + direction * finalDistance;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followLerpSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}
