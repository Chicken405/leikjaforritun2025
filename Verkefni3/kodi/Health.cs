using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int health;
    private static GameManager instance;

    void Start()
    {
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        if (!gameManager)
        {
            Debug.LogError("No GameManager found! Very bad!!!");
        }
        instance = gameManager.GetComponent<GameManager>();
        health = maxHealth;
        // Ef player
        if (this.CompareTag("Player"))
        {
            // Updatar UI
            HealthUIScript healthUIScript = instance.GetComponent<HealthUIScript>();
            healthUIScript.SetHealth(health);
        }
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        // Ef player
        if (this.CompareTag("Player"))
        {
            // Updatar UI
            HealthUIScript healthUIScript = instance.GetComponent<HealthUIScript>();
            healthUIScript.SetHealth(health);
        }
        CheckHealth();
    }
    public void CheckHealth()
    {
        // Ef health undir 0
        if (health <= 0)
        {
            if (this.CompareTag("Enemy"))
            {
                // Drepur enemy
                EnemyScript enemyScript = gameObject.GetComponent<EnemyScript>();
                enemyScript.DestroyEnemy();
            }
            else
            {
                // Call á death sena
                instance.CallDeath();
            }
        }
    }
}
