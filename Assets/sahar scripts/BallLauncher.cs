using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    [Header("Identity")]
    public TurnManager.Side side = TurnManager.Side.Player1;

    [Header("Ball Settings")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Vector2 launchDirection = new Vector2(1f, 2f);

    [Header("Charge Settings")]
    [SerializeField] private float minForce = 10f;
    [SerializeField] private float maxForce = 50f;
    [SerializeField] private float timeToMax = 1.0f; // seconds to reach full power

    [Header("Audio")]
    [SerializeField] private AudioClip throwSfx;
    [SerializeField, Range(0f,1f)] private float throwSfxVolume = 1f;
    [SerializeField] private AudioClip chargeLoopSfx;                 // <-- new (loop while holding)
    [SerializeField, Range(0f,1f)] private float chargeLoopVolume = 0.8f;

    [Header("UI")]
    [SerializeField] private ChargeBar chargeBar;

    private float chargeStartTime;
    private bool isCharging = false;

    // private loop source (auto-added on Awake)
    private AudioSource chargeSource;

    void Awake()
    {
        chargeSource = GetComponent<AudioSource>();
        if (!chargeSource) chargeSource = gameObject.AddComponent<AudioSource>();
        chargeSource.playOnAwake = false;
        chargeSource.loop = true;          // loop while charging
        chargeSource.spatialBlend = 0f;    // 2D sound (set to 1 for 3D if you prefer)
    }

    // Bind to your Shoot action (Button)
    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            // only start charging (and audio) on our turn
            if (TurnManager.I != null && TurnManager.I.CanShoot(side))
            {
                isCharging = true;
                chargeStartTime = Time.time;
                chargeBar?.UpdateCharge(0f);
                StartChargeLoop();
            }
        }
        else if (ctx.canceled)
        {
            if (!isCharging) return;

            float held = Time.time - chargeStartTime;
            float t = (timeToMax <= 0f) ? 1f : Mathf.Clamp01(held / timeToMax);
            float force = Mathf.Lerp(minForce, maxForce, t);

            StopChargeLoop(); // stop the loop the moment we throw

            // play throw SFX only if it's still our turn
            if (TurnManager.I != null && TurnManager.I.CanShoot(side) && throwSfx)
                SoundEffectsManager.instance?.PlaySoundEffectsClip(throwSfx, transform, throwSfxVolume);

            FireBall(force);

            isCharging = false;
            chargeBar?.HideAndReset();
        }
    }

    void Update()
    {
        if (!isCharging) return;

        // safety: if somehow turn became invalid while holding, cancel & stop audio
        if (TurnManager.I != null && !TurnManager.I.CanShoot(side))
        {
            isCharging = false;
            StopChargeLoop();
            chargeBar?.HideAndReset();
            return;
        }

        float held = Time.time - chargeStartTime;
        float t = (timeToMax <= 0f) ? 1f : Mathf.Clamp01(held / timeToMax);
        chargeBar?.UpdateCharge(t);

        // (optional) ramp pitch with charge for a nicer feel:
        if (chargeSource && chargeSource.isPlaying) chargeSource.pitch = Mathf.Lerp(0.9f, 1.2f, t);
    }

    private void FireBall(float force)
    {
        GameObject ball = Instantiate(ballPrefab, firePoint.position, Quaternion.identity);

        if (ball.TryGetComponent<Rigidbody2D>(out var rb))
        {
#if UNITY_6000_0_OR_NEWER || UNITY_2023_1_OR_NEWER
            rb.linearVelocity = launchDirection.normalized * force;
#else
            rb.velocity = launchDirection.normalized * force;
#endif
        }

        TurnManager.I?.NotifyShotFired(side);

        if (!ball.TryGetComponent<Ball>(out var b))
            b = ball.AddComponent<Ball>();
        b.enableTurnNotify = true;
    }

    // ---- audio helpers ----
    private void StartChargeLoop()
    {
        if (!chargeLoopSfx) return;
        chargeSource.clip = chargeLoopSfx;
        chargeSource.volume = chargeLoopVolume;
        if (!chargeSource.isPlaying) chargeSource.Play();
    }

    private void StopChargeLoop()
    {
        if (chargeSource && chargeSource.isPlaying) chargeSource.Stop();
        chargeSource.clip = null; // free reference
        chargeSource.pitch = 1f;  // reset
    }
}


