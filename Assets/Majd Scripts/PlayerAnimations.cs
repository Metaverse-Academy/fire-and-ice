using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BallLauncher))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private BallLauncher launcher;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        launcher = GetComponent<BallLauncher>();
    }

    private void Update()
    {
        // Animate Throwing while charging
        animator.SetBool("IsThrowing", launcherIsCharging());
    }

    // External triggers for damage/death
    public void PlayHurt()
    {
        animator.SetTrigger("Hurt");
    }

    public void PlayDie()
    {
        animator.SetBool("Dead", true);
    }

    // Use reflection to read private isCharging field without modifying BallLauncher
    private bool launcherIsCharging()
    {
        var field = typeof(BallLauncher).GetField("isCharging", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)field.GetValue(launcher);
    }
}
