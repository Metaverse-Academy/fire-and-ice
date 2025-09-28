using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [Header("Damage & Lifetime (charge = more distance ONLY)")]
    [SerializeField] private int damage = 1;              // fixed damage
    [SerializeField] private float baseLifeTime = 2.5f;   // seconds at no charge
    [SerializeField] private float extraLifeAtFull = 2.0f;// extra seconds at full charge

    [Header("Homing / Steering")]
    [SerializeField] private float turnDegreesPerSecond = 260f; // higher = tighter curve
    [SerializeField] private float maxSpeed = 24f;
    [SerializeField] private float acceleration = 0f;     // 0 = keep speed
    [SerializeField] private float gravityScale = 0f;     // 0 = pure homing

    [Header("Walls / Avoidance")]
    [SerializeField] private LayerMask wallLayers;        // assign your Wall layer(s)
    [SerializeField] private float lookAhead = 1.1f;      // forward ray length
    [SerializeField] private float sideProbe = 0.6f;      // side offset for probes
    [SerializeField] private float avoidTurn = 40f;       // steer away angle when blocked

    private Rigidbody2D rb;
    private int ownerPlayerIndex = -1;
    private Transform target;
    private float currentSpeed;

    // convenience overload
    public void Initialize(int ownerPlayerIndex, Vector2 velocity)
    {
        Initialize(ownerPlayerIndex, velocity, null, 0f);
    }

    public void Initialize(int ownerPlayerIndex, Vector2 initialVelocity, Transform target, float charge01)
    {
        this.ownerPlayerIndex = ownerPlayerIndex;
        this.target = target;

        if (!rb) rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScale;
        rb.linearVelocity = initialVelocity;
        currentSpeed = initialVelocity.magnitude;

        // charge -> longer lifetime (distance)
        float life = baseLifeTime + Mathf.Clamp01(charge01) * extraLifeAtFull;
        Destroy(gameObject, life);

        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!rb) return;

        Vector2 v = rb.linearVelocity;
        if (v.sqrMagnitude < 0.0001f) v = transform.right * 0.01f;

        // desired = toward target (if any)
        Vector2 desiredDir = v.normalized;
        if (target != null)
        {
            Vector2 toTarget = ((Vector2)target.position - rb.position).normalized;
            desiredDir = toTarget;
        }

        // avoidance if wall ahead
        if (Physics2D.Raycast(rb.position, v.normalized, lookAhead, wallLayers))
        {
            Vector2 left  = Quaternion.Euler(0, 0,  90f) * v.normalized;
            Vector2 right = Quaternion.Euler(0, 0, -90f) * v.normalized;
            bool leftBlocked  = Physics2D.Raycast(rb.position + left  * sideProbe, v.normalized, lookAhead, wallLayers);
            bool rightBlocked = Physics2D.Raycast(rb.position + right * sideProbe, v.normalized, lookAhead, wallLayers);

            float sign = (rightBlocked && !leftBlocked) ? 1f :
                         (!rightBlocked && leftBlocked) ? -1f :
                         (Random.value < 0.5f ? 1f : -1f);

            desiredDir = Quaternion.Euler(0, 0, sign * avoidTurn) * v.normalized;
        }

        // rotate velocity toward desired
        float maxRad = turnDegreesPerSecond * Mathf.Deg2Rad * Time.fixedDeltaTime;
        Vector2 newDir = Vector3.RotateTowards(v.normalized, desiredDir, maxRad, 0f);

        currentSpeed = Mathf.Min(maxSpeed, v.magnitude + acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = newDir * currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1) touch wall -> disappear
        if (((1 << other.gameObject.layer) & wallLayers) != 0)
        {
            Destroy(gameObject);
            return;
        }

        // 2) ignore shooter
        var otherPI = other.GetComponent<PlayerInput>();
        if (otherPI != null && otherPI.playerIndex == ownerPlayerIndex)
            return;

        // 3) damage the other player + debug
        var hp = other.GetComponent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(damage);

            string who = (otherPI != null) ? $"Player {otherPI.playerIndex + 1}" : "Player ?";
            Debug.Log($"{who} got damaged ({damage}).");

            Destroy(gameObject);
        }
    }
}


