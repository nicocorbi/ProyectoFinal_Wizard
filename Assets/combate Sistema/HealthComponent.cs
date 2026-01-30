using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 100;
    public int currentHealth = 0;

    public System.Action OnDeath;
    public System.Action<int> OnDamage;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        OnDamage?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}

