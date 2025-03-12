using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour
{
    public float maxHealth = 150f;             // Maximum health for the player
    private float currentHealth;               // Current health of the player

    private void Start()
    {
        currentHealth = maxHealth;             // Initialize health to max at start
        Debug.Log("Player Health Initialized: " + currentHealth);
    }

    // Method to deal damage to the player
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Took Damage: " + damage + " | Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    // Method to handle player death
    private void HandleDeath()
    {
        Debug.Log("Player has died!");
        // Here you can add respawn logic or other effects if needed.
    }
}
