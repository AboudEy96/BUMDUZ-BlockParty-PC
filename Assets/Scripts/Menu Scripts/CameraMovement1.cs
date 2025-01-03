using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 1f;
    public float radius = 25f;

    private float angle = 0f;
    private bool isMovingRight = true;

    private void Update()
    {
        float deltaAngle = speed * Time.deltaTime;

        if (isMovingRight)
        {
            angle += deltaAngle;
            if (angle > 50f)
            {
                angle = 50f;
                isMovingRight = false;
            }
        }
        else
        {
            angle -= deltaAngle;
            if (angle < -50f)
            {
                angle = -50f;
                isMovingRight = true;
            }
        }

        float x = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        transform.position = new Vector3(x, transform.position.y, z);
        transform.LookAt(Vector3.zero);
    }
}
