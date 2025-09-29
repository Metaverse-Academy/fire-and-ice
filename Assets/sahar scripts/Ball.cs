using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var hp = collision.gameObject.GetComponent<Health>()
                     ?? collision.gameObject.GetComponentInParent<Health>();
            if (hp != null)
                hp.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}


