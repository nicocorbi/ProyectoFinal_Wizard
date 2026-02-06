using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 100;
    public int currentHealth = 0;

    public System.Action OnDeath;
    public System.Action<int> OnDamage;
    public System.Action<int, int> OnHealthChanged;
  

    private void Awake()
    {
        currentHealth = maxHealth;

        // Notificar vida inicial
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Notificar daño
        OnDamage?.Invoke(currentHealth);

        // Notificar cambio de vida
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        // Notificar cambio de vida
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
