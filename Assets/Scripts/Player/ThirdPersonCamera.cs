using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    public float distance = 6f;
    public float height = 1.5f;
    public float mouseSensitivity = 2f;
    public float minPitch = -20f;
    public float maxPitch = 65f;

    private float yaw;
    private float pitch = 15f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Camera target is missing!");
            enabled = false;
            return;
        }

        yaw = target.eulerAngles.y;
        LockCursor();
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        }

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 cameraOffset = rotation * new Vector3(0, 0, -distance);

        Vector3 lookTarget = target.position + Vector3.up * height;

        transform.position = lookTarget + cameraOffset;

        transform.LookAt(lookTarget);
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
