using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject goldPrefab;
    public GameObject smallEnemyPrefab;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} b? trúng ð?n! HP c?n: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // G?i animation ch?t (n?u có)
        Boss1Controller controller = GetComponent<Boss1Controller>();
        if (controller != null)
        {
            controller.OnDeath();
        }

        // Rõi 1 vàng
        if (goldPrefab != null)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }

        // Spawn 3 enemy nh?
        if (smallEnemyPrefab != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 spawnPos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                Instantiate(smallEnemyPrefab, spawnPos, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}
