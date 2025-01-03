using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0, 5, -10); // fov
    public float rotationSpeed = 100f; // sentivity

    private float currentYaw = 0f;
    void LateUpdate()
    {
        if (target == null) return;
        
        currentYaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        transform.position = target.position + Quaternion.Euler(0, currentYaw, 0) * offset;

        transform.LookAt(target);
    }
}