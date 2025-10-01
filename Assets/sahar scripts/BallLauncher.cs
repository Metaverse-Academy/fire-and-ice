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

    [Header("UI")]
    [SerializeField] private ChargeBar chargeBar;    // drag P1 or P2 bar here
    [SerializeField] private AudioClip[] ballthrowClips;


    private float chargeStartTime;
    private bool isCharging = false;

    // Bind to your Shoot action (Button): started=press, canceled=release
    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (TurnManager.I != null && TurnManager.I.CanShoot(side))
            {
                isCharging = true;
                chargeStartTime = Time.time;
                chargeBar?.UpdateCharge(0f); // show at 0
            }
        }
        else if (ctx.canceled)
        {
            if (!isCharging) return;

            float held = Time.time - chargeStartTime;
            float t = (timeToMax <= 0f) ? 1f : Mathf.Clamp01(held / timeToMax);
            float force = Mathf.Lerp(minForce, maxForce, t);

            FireBall(force);

            isCharging = false;
            chargeBar?.HideAndReset();
        }
                SoundEffectsManager.instance.PlayRandomSoundEffectsClip(ballthrowClips, transform, 1f); //is it here where player shoots the bullet? raneem

    }

    void Update()
    {
        if (!isCharging) return;

        float held = Time.time - chargeStartTime;
        float t = (timeToMax <= 0f) ? 1f : Mathf.Clamp01(held / timeToMax);

        // UI only moves while holding → fills UP from bottom
        chargeBar?.UpdateCharge(t);
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

        // ensure ball notifies end of turn
        if (!ball.TryGetComponent<Ball>(out var b))
            b = ball.AddComponent<Ball>();
        b.enableTurnNotify = true;
    }
}
