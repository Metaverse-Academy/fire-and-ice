using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 5f;

    [HideInInspector] public bool enableTurnNotify = false;
     [SerializeField] private AudioClip[] ballhitClips;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var hp = other.GetComponent<Health>() ?? other.GetComponentInParent<Health>();
            if (hp != null) hp.TakeDamage(damage);
            Destroy(gameObject);
        }

        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
        SoundEffectsManager.instance.PlayRandomSoundEffectsClip(ballhitClips, transform, 1f);
    }

    void OnDestroy()
    {
        if (enableTurnNotify)
            TurnManager.I?.NotifyBallEnded();
    }
}

