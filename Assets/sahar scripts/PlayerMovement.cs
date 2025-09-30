// using UnityEngine; 
// using UnityEngine.InputSystem;

// [RequireComponent(typeof(Rigidbody2D))]
// [RequireComponent(typeof(PlayerInput))]
// public class PlayerMovement : MonoBehaviour
// {
//     public enum AllowedShotDirection { Left = -1, Right = 1 }

//     [Header("Movement")]
//     [SerializeField] private float moveSpeed = 6f;
//     [SerializeField] private Transform groundCheckTransform;
//     [SerializeField] private LayerMask groundLayer;
//     [SerializeField] private float checkRadius = 0.15f;
//     [SerializeField] private float jumpForce = 13f;

//     [Header("Shooting / Charge")]
//     [SerializeField] private GameObject projectilePrefab;
//     [SerializeField] private Transform projectileSpawn;
//     [SerializeField] private float launchSpeed = 14f;      // keep constant so charge only affects distance
//     [SerializeField] private float maxChargeTime = 1.0f;   // hold to get more distance
//     [SerializeField] private float postShotCooldown = 0.15f;
//     [Tooltip("x = forward; y = up. Set y = 0 for straight shot.")]
//     [SerializeField] private Vector2 launchAngle = new Vector2(1f, 0f);

//     [Header("Shooting Rules")]
//     [SerializeField] private AllowedShotDirection allowedShotDirection = AllowedShotDirection.Right;
//     // Set left player's to Right; right player's to Left.

//     private Rigidbody2D rb;
//     private Vector2 moveInput;
//     private bool isGrounded;

//     // charge
//     private bool isCharging = false;
//     private float chargeStart = 0f;
//     private float nextAllowedShootTime = 0f;

//     // for UI bars (0..1 while holding)
//     public float CurrentCharge01 { get; private set; } = 0f;

//     // owner id for friendly-fire avoidance
//     public int PlayerIndex { get; private set; } = -1;

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         var pi = GetComponent<PlayerInput>();
//         if (pi != null) PlayerIndex = pi.playerIndex;
//     }

//     private void Update()
//     {
//         isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, checkRadius, groundLayer);
//         if (isCharging)
//             CurrentCharge01 = Mathf.Clamp01((Time.time - chargeStart) / maxChargeTime);
//     }

//     private void FixedUpdate()
//     {
//         // Unity 6 horizontal helper
//         rb.linearVelocityX = moveInput.x * moveSpeed;
//     }

//     // === Input System callbacks ===
//     public void Moving(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();

//     public void Jumping(InputAction.CallbackContext ctx)
//     {
//         if (ctx.performed && isGrounded)
//             rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//     }

//     // Bind in your Input Actions as "Fire" (Button)
//     public void Fire(InputAction.CallbackContext ctx)
//     {
//         if (ctx.started && Time.time >= nextAllowedShootTime)
//         {
//             isCharging = true;
//             chargeStart = Time.time;
//             CurrentCharge01 = 0f;
//         }
//         else if (ctx.canceled && isCharging)
//         {
//             float t = Mathf.Clamp01((Time.time - chargeStart) / maxChargeTime); // 0..1
//             Shoot(launchSpeed, t); // speed constant; t will extend projectile lifetime
//             isCharging = false;
//             CurrentCharge01 = 0f;
//             nextAllowedShootTime = Time.time + postShotCooldown;
//         }
//     }

//     // === YOUR UPDATED SHOOT METHOD (forced direction) ===
//     private void Shoot(float speed, float charge01)
//     {
//         if (!projectilePrefab || !projectileSpawn) return;

//         // FORCE shot direction: left player -> Right, right player -> Left
//         int sign = (int)allowedShotDirection; // +1 or -1
//         Vector2 dir = new Vector2(sign * Mathf.Abs(launchAngle.x), launchAngle.y).normalized;

//         GameObject go = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
//         var proj = go.GetComponent<Projectile>();
//         if (proj != null)
//         {
//             Transform target = FindOtherPlayer();
//             proj.Initialize(PlayerIndex, dir * speed, target, charge01);
//         }
          
//     }

//     private Transform FindOtherPlayer()
//     {
//         var all = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);
//         foreach (var pi in all)
//             if (pi.playerIndex != PlayerIndex) return pi.transform;
//         return null;
//     }

//     private void OnDrawGizmos()
//     {
//         if (!groundCheckTransform) return;
//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireSphere(groundCheckTransform.position, checkRadius);
//     }
//     // inside PlayerMovement
// public event System.Action OnShot;



// }




