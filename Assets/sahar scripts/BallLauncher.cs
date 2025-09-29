using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float launchForce = 10f;

    [Header("Launch Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Vector2 launchDirection = new Vector2(1, 2);


    public void OnBallTrigger(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        FireBall();
    }

    void FireBall()
    {
        GameObject ball = Instantiate(ballPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = launchDirection.normalized * launchForce;
        }
    }
}

