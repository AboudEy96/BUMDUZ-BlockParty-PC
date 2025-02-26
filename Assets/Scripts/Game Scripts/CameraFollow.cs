using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10); // fov
    public float rotationSpeed = 100f; // sensitivity
    public float shakeAmount = 0.5f; // shake up/down
    private float currentYaw = 0f;
    private float currentPitch = 0f; // دوران الكاميرا للأعلى والأسفل
    public float pitchLimit = 45f; // الحد الأقصى للدوران العمودي

    // fp, sp
    public bool firstPerson;
    public Transform targetHead;

    void LateUpdate()
    {
        if (target == null) return;
        if (targetHead == null) return;

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        currentYaw += mouseX;

        if (firstPerson)
        {
            currentPitch -= mouseY;
            currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);
        }

        if (firstPerson)
        {
            transform.position = targetHead.position;

            transform.position += new Vector3(0, Mathf.Sin(Time.time * 5) * shakeAmount, 0);
            transform.position += new Vector3(Mathf.PerlinNoise(Time.time * 0.2f, 0) - 0.5f,
                Mathf.PerlinNoise(0, Time.time * 0.2f) - 0.5f, 0) * shakeAmount;

            transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        }
        else
        {
            Vector3 desiredPosition = target.position + Quaternion.Euler(0, currentYaw, 0) * offset;
            transform.position = desiredPosition;

            transform.LookAt(target);
        }
    }

    void AssignLayer(Transform trans, int layerIndex)
    {
        trans.gameObject.layer = layerIndex;
        foreach (Transform child in trans)
        {
            AssignLayer(child, layerIndex);
        }
    }
}
