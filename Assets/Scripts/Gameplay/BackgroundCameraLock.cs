using UnityEngine;

[DisallowMultipleComponent]
public class BackgroundCameraLock : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = true;
    [SerializeField] private Vector2 parallaxFactor = new Vector2(0.2f, 0.08f);

    private Vector3 initialWorldPosition;
    private Vector3 initialCameraPosition;
    private bool initialized;

    private void Start()
    {
        Initialize();
        ApplyLock();
    }

    private void LateUpdate()
    {
        ApplyLock();
    }

    public void Configure(Camera cameraTarget, bool shouldFollowX, bool shouldFollowY, Vector2 targetParallaxFactor)
    {
        targetCamera = cameraTarget;
        followX = shouldFollowX;
        followY = shouldFollowY;
        parallaxFactor = targetParallaxFactor;
        initialized = false;
        Initialize();
        ApplyLock();
    }

    private void Initialize()
    {
        if (initialized)
        {
            return;
        }

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera == null)
        {
            return;
        }

        initialWorldPosition = transform.position;
        initialCameraPosition = targetCamera.transform.position;
        initialized = true;
    }

    private void ApplyLock()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera == null)
        {
            return;
        }

        if (!initialized)
        {
            Initialize();
        }

        Vector3 position = initialWorldPosition;
        Vector3 cameraDelta = targetCamera.transform.position - initialCameraPosition;

        if (followX)
        {
            position.x += cameraDelta.x * parallaxFactor.x;
        }

        if (followY)
        {
            position.y += cameraDelta.y * parallaxFactor.y;
        }

        transform.position = position;
    }
}
