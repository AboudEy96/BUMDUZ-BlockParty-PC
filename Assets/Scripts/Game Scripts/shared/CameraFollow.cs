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

    private float currentYaw   = 0f;
    private float currentPitch = 20f;
    private float currentExtraDistance = 0f;

    private const float PITCH_MIN = -10f;
    private const float PITCH_MAX = 60f;

    private Vector2 _lastTouchPos;
    private bool _isTouching = false;

    private bool _pitchEnabled = false;
    
    public void SetPitchEnabled(bool enabled)
    {
        _pitchEnabled = enabled;
    }

    private void Start()
    {
        rotationSpeed = PlayerPrefs.GetFloat("Settings_MouseSpeed", 100f);

        _pitchEnabled = PlayerPrefs.GetInt("Settings_CameraPitch", 0) == 1;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                target = player.transform;
                break;
            }
        }

        photonView  = target?.GetComponent<PhotonView>();
        targetPlayer = target?.GetComponent<Player>();

        if (target == null || photonView == null || !photonView.IsMine)
            gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (target == null || photonView == null || !photonView.IsMine) return;

        float mouseX = 0f;
        float mouseY = 0f;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _lastTouchPos = touch.position;
                    _isTouching   = true;
                    break;

                case TouchPhase.Moved:
                    if (_isTouching)
                    {
                        Vector2 delta = touch.position - _lastTouchPos;
                        mouseX = delta.x * (rotationSpeed / 1000f);
                        mouseY = delta.y * (rotationSpeed / 1000f);
                        _lastTouchPos = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    _isTouching = false;
                    break;
            }
        }
        // PC Mouse
        else
        {
            mouseX =  Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        }
        
        currentYaw += mouseX;
        
        if (_pitchEnabled)
        {
            currentPitch += mouseY;
            currentPitch  = Mathf.Clamp(currentPitch, PITCH_MIN, PITCH_MAX);
        }
        bool isRunning = targetPlayer != null && targetPlayer.IsRunning;
        float targetExtra = isRunning ? runExtraDistance : 0f;
        currentExtraDistance = Mathf.Lerp(currentExtraDistance, targetExtra, runLerpSpeed * Time.deltaTime);

        Quaternion rotation  = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3    direction = (rotation * Vector3.back).normalized;
        float      distance  = offset.magnitude + currentExtraDistance;

        Vector3 desiredPosition = target.position + rotation * new Vector3(0f, offset.y * 0.5f, 0f) + direction * distance;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followLerpSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}