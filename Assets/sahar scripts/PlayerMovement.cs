using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius = 0.15f;
    [SerializeField] private float jumpForce = 13f;

    [Header("Shooting / Charge")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private float launchSpeed = 14f;     // constant speed
    [SerializeField] private float maxChargeTime = 1.0f;  // hold time to reach full range
    [SerializeField] private float postShotCooldown = 0.15f;
    [Tooltip("x = forward; y = up. Set y = 0 for straight shot.")]
    [SerializeField] private Vector2 launchAngle = new Vector2(1f, 0f);

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;

    // charge
    private bool isCharging = false;
    private float chargeStart = 0f;
    private float nextAllowedShootTime = 0f;

    // facing (1 right, -1 left)
    private int facingDir = 1;

    // for UI bars
    public float CurrentCharge01 { get; private set; } = 0f;

    // owner id for friendly-fire avoidance
    public int PlayerIndex { get; private set; } = -1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        var pi = GetComponent<PlayerInput>();
        if (pi != null) PlayerIndex = pi.playerIndex;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, checkRadius, groundLayer);
        if (isCharging)
            CurrentCharge01 = Mathf.Clamp01((Time.time - chargeStart) / maxChargeTime);
    }

    private void FixedUpdate()
    {
        // Unity 6 helper: horizontal only
        rb.linearVelocityX = moveInput.x * moveSpeed;

        if (moveInput.x > 0.01f)      facingDir = 1;
        else if (moveInput.x < -0.01f) facingDir = -1;

        // optional flip
        var s = transform.localScale;
        transform.localScale = new Vector3(Mathf.Abs(s.x) * facingDir, s.y, s.z);
    }

    // Input System callbacks
    public void Moving(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();

    public void Jumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // Bind this to your "Fire" action (Button)
    public void Fire(InputAction.CallbackContext ctx)
    {
        if (ctx.started && Time.time >= nextAllowedShootTime)
        {
            isCharging = true;
            chargeStart = Time.time;
            CurrentCharge01 = 0f;
        }
        else if (ctx.canceled && isCharging)
        {
            float t = Mathf.Clamp01((Time.time - chargeStart) / maxChargeTime); // 0..1
            Shoot(launchSpeed, t);  // speed constant; t goes to projectile lifetime

            isCharging = false;
            CurrentCharge01 = 0f;
            nextAllowedShootTime = Time.time + postShotCooldown;
        }
    }

    private void Shoot(float speed, float charge01)
    {
        if (!projectilePrefab || !projectileSpawn) return;

        Vector2 dir = new Vector2(facingDir * Mathf.Abs(launchAngle.x), launchAngle.y).normalized;

        GameObject go = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        var proj = go.GetComponent<Projectile>();
        if (proj != null)
        {
            Transform target = FindOtherPlayer();
            proj.Initialize(PlayerIndex, dir * speed, target, charge01);
        }
    }

    private Transform FindOtherPlayer()
    {
        var all = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);
        foreach (var pi in all)
            if (pi.playerIndex != PlayerIndex) return pi.transform;
        return null;
    }

    private void OnDrawGizmos()
    {
        if (!groundCheckTransform) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckTransform.position, checkRadius);
    }
}
