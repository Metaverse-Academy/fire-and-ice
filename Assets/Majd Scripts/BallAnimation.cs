using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class BallAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Rotation Settings")]
    [Tooltip("Extra angle offset if your sprite doesn’t line up by default")]
    [SerializeField] private float angleOffset = 0f;

    [Header("Destroy Timing")]
    [Tooltip("Time in seconds to wait before destroying after explosion")]
    [SerializeField] private float explodeDuration = 0.5f;

    private bool hasExploded = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Start with Shoot animation immediately
        animator.Play("Shoot", -1, 0f);
    }

    void LateUpdate()
    {
        // Rotate the sprite to follow velocity
        if (rb.linearVelocity.sqrMagnitude > 0.01f && !hasExploded)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward);
        }
    }

    public void PlayExplode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Play the explosion animation
        animator.Play("Explode", -1, 0f);

        // Stop physics so the ball doesn’t keep moving
        if (rb) rb.simulated = false;

        // Destroy after the animation
        Destroy(gameObject, explodeDuration);
    }
}
