using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.2f, -10f);
    public float smoothTime = 0.12f;

    private Vector3 velocity;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = offset.z;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}
