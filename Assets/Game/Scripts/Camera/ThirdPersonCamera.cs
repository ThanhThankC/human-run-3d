using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.6f, 0f);

    [Header("Distance")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private float maxDistance = 8f;

    [Header("Rotation")]
    [SerializeField] private float sensitivityX = 3f;
    [SerializeField] private float sensitivityY = 2f;
    [SerializeField] private float minPitch = -10f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Collision")]
    [SerializeField] private float collisionRadius = 0.2f;
    [SerializeField] private float collisionOffest = 0.1f;
    [SerializeField] private LayerMask collisionMask;

    [Header("Smoothing")]
    [SerializeField] private float positionSmooth = 10f;

    private float yaw;
    private float pitch = -20f;
    private float currentDistance;

    private const float DefaultSensitivityMobile = 0.1f;

    private void Start()
    {
        yaw = transform.eulerAngles.y;
        currentDistance = distance;
#if UNITY_EDITOR || UNITY_STANDALONE
        Cursor.lockState = CursorLockMode.Locked;
#else
        Cursor.lockState = CursorLockMode.None;
#endif
    }

    private void Update()
    {
        HandleInput();
    }

    private void LateUpdate()
    {
        ApplyCamera();
    }

    private void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        yaw += Input.GetAxis("Mouse X") * sensitivityX;
        pitch -= Input.GetAxis("Mouse Y") * sensitivityY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
#else
        for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
 
                if (touch.position.x < Screen.width * 0.5f) continue;
 
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.deltaPosition / Time.deltaTime;
 
                    yaw   += delta.x * sensitivityX * DefaultSensitivityMobile * Time.deltaTime;
                    pitch -= delta.y * sensitivityY * DefaultSensitivityMobile * Time.deltaTime;
                    pitch  = Mathf.Clamp(pitch, minPitch, maxPitch);
                }
            }
#endif
    }

    private void ApplyCamera()
    {
        if (target == null) return;

        Vector3 pivotPos = target.position + targetOffset;
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredDir = rotation * Vector3.back;

        float targetDistance = distance;
        if (Physics.SphereCast(pivotPos, collisionRadius, desiredDir, out var hit, distance, collisionMask))
            targetDistance = Mathf.Clamp(hit.distance - collisionOffest, minDistance, maxDistance);

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, positionSmooth * Time.deltaTime);
        Vector3 desiredPos = pivotPos + desiredDir * currentDistance;

        transform.position = Vector3.Lerp(transform.position, desiredPos, positionSmooth * Time.deltaTime);
        transform.LookAt(pivotPos);
    }
}