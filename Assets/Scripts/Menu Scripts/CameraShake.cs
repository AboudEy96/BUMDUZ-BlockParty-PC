using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.1f;

    private float shakeTimer = 0f;
    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
            transform.position = originalPos;
        }
    }

    public void TriggerShake()
    {
        shakeTimer = shakeDuration;
    }
}
