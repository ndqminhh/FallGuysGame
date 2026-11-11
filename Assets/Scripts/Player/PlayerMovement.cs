
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpHeight = 2.5f;
    public float gravity = -20f;
    public float rotationSpeed = 12f;

    [Header("References")]
    public Transform cameraTransform;

    [Header("Respawn")]
    public float fallLimit = -5f;

    [Header("Knockback")]
    public float knockbackDecay = 8f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 knockbackVelocity;
    private Vector3 spawnPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        spawnPosition = transform.position;

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        bool grounded = controller.isGrounded;

        if (grounded && velocity.y < 0f)
            velocity.y = -2f;

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = Vector3.zero;

        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            direction = (forward * vertical + right * horizontal).normalized;
        }

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 movement = direction * moveSpeed + knockbackVelocity + Vector3.up * velocity.y;

        controller.Move(movement * Time.deltaTime);

        knockbackVelocity = Vector3.MoveTowards(knockbackVelocity, Vector3.zero, knockbackDecay * Time.deltaTime);

        if (transform.position.y < fallLimit)
            Respawn();
    }

    public void ApplyKnockback(Vector3 direction, float strength)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        knockbackVelocity = direction.normalized * strength;

        velocity.y = Mathf.Max(velocity.y, 4.5f);
    }

    public void SetCheckpoint(Vector3 position)
    {
        spawnPosition = position;
    }

    public void Respawn()
    {
        controller.enabled = false;

        transform.SetParent(null, true);

        transform.position = spawnPosition;
        velocity = Vector3.zero;
        knockbackVelocity = Vector3.zero;

        controller.enabled = true;
    }
}
