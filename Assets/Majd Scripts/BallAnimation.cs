using UnityEngine;

public class BallAnimation : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayExplode()
    {
        if (animator != null)
        {
            animator.SetTrigger("Explode");
            Destroy(gameObject, 0.5f); // delay so explosion plays
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
