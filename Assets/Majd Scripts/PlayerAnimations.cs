using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement movement;

    private bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();

        // Subscribe to shooting event
        movement.OnShot += PlayThrow;
    }

    private void OnDestroy()
    {
        movement.OnShot -= PlayThrow;
    }

    // === Animation Triggers ===
    private void PlayThrow()
    {
        if (isDead) return;
        anim.SetTrigger("Throw");
    }

    public void PlayHurt()
    {
        if (isDead) return;
        anim.SetTrigger("Hurt");
    }

    public void PlayDie()
    {
        if (isDead) return;
        isDead = true;
        anim.SetBool("Dead", true);
    }
}
