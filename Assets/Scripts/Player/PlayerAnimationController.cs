
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;

    [Header("Animation Settings")]
    public float runSpeed = 7f;
    public float airborneGraceTime = 0.12f;

    private CharacterController controller;
    private float airborneTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void LateUpdate()
    {
        if (animator == null) return;

        bool grounded = controller.isGrounded;

        if (grounded)
            airborneTimer = 0f;
        else
            airborneTimer += Time.deltaTime;

        bool stableGrounded = grounded || airborneTimer < airborneGraceTime;

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        float speed = Mathf.Clamp01(horizontalVelocity.magnitude / Mathf.Max(runSpeed, 0.01f));

        if (GameManager.Instance != null && GameManager.Instance.IsFinished)
        {
            speed = 0f;
        }

        animator.SetFloat("Speed", speed, 0.12f, Time.deltaTime);

        animator.SetBool("Grounded", stableGrounded);
    }
}
