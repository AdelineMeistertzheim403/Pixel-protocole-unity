using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(1000)]
public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.2f, -10f);
    public float smoothTime = 0.2f;
    public Vector2 deadZone = new Vector2(1.25f, 0.9f);
    public bool followX = true;
    public bool followY = false;

    private Vector3 velocity;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsureCameraFollowOnMainCamera()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (!activeScene.IsValid())
        {
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return;
        }

        CameraFollow2D follow = mainCamera.GetComponent<CameraFollow2D>();
        if (follow == null)
        {
            follow = mainCamera.gameObject.AddComponent<CameraFollow2D>();
        }

        follow.TryAssignTarget();
        follow.SnapToTarget();
    }

    private void Start()
    {
        TryAssignTarget();
        SnapToTarget();
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
        SnapToTarget();
    }

    void LateUpdate()
    {
        TryAssignTarget();

        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = transform.position;
        Vector3 targetPosition = target.position + offset;

        if (followX && Mathf.Abs(targetPosition.x - transform.position.x) > deadZone.x)
        {
            desiredPosition.x = targetPosition.x;
        }

        if (followY && Mathf.Abs(targetPosition.y - transform.position.y) > deadZone.y)
        {
            desiredPosition.y = targetPosition.y;
        }

        desiredPosition.z = offset.z;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }

    private void TryAssignTarget()
    {
        if (target != null)
        {
            return;
        }

        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            target = player.transform;
            return;
        }

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
    }

    private void SnapToTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector3 snappedPosition = target.position + offset;
        snappedPosition.z = offset.z;
        transform.position = snappedPosition;
    }
}
