using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    public int CurrentHealth { get; private set; }

    public UnityEvent onDamaged;
    public UnityEvent onDeath;

    void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        onDamaged?.Invoke();

        if (CurrentHealth <= 0)
        {
            onDeath?.Invoke();
            // Default behavior; replace with respawn/round-end as needed:
            gameObject.SetActive(false);
        }
    }

    public void Heal(int amount)
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
    }

    // Optional helper if you need to reset between rounds:
    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
        if (!gameObject.activeSelf) gameObject.SetActive(true);
    }
}
