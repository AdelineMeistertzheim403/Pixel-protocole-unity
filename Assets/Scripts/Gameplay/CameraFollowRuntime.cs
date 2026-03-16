using UnityEngine;

public class CameraFollowRuntime : MonoBehaviour
{
    private static CameraFollowRuntime instance;

    [SerializeField] private Vector3 offset = new Vector3(0f, 1.1f, -10f);
    [SerializeField] private float smoothTime = 0.1f;

    private Camera targetCamera;
    private Transform target;
    private Vector3 velocity;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (instance != null)
        {
            return;
        }

        GameObject runtimeObject = new GameObject("CameraFollowRuntime");
        DontDestroyOnLoad(runtimeObject);
        instance = runtimeObject.AddComponent<CameraFollowRuntime>();
    }

    private void LateUpdate()
    {
        ResolveCameraAndTarget();
        if (targetCamera == null || target == null)
        {
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = offset.z;
        targetCamera.transform.position = Vector3.SmoothDamp(
            targetCamera.transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );
    }

    private void ResolveCameraAndTarget()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (target == null)
        {
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
    }
}
