using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Cài đặt máu")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    public int CurrentHealth => currentHealth;

    [Header("Thanh máu")]
    public EnemyHealthBar healthBar;  // Gắn từ Inspector

    [Header("Prefab Drop")]
    public GameObject goldPrefab;
    public GameObject smallEnemyPrefab;
    public GameObject healthPickupPrefab;
    public GameObject manaPickupPrefab; // 👈 Prefab mana

    [Header("Âm thanh")]
    public AudioClip hurtClip;
    public AudioClip deathClip;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} bị trúng đòn! HP còn: {currentHealth}");

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (hurtClip != null && audioSource != null)
            audioSource.PlayOneShot(hurtClip);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        if (deathClip != null && audioSource != null)
            audioSource.PlayOneShot(deathClip);

        // Rơi vàng
        if (goldPrefab != null)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }

        // Rơi cục máu (máu hồi cho Player)
        if (healthPickupPrefab != null)
        {
            Vector3 offset = new Vector3(0.4f, 0.3f, 0);
            Instantiate(healthPickupPrefab, transform.position + offset, Quaternion.identity);
        }

        // Rơi mana
        if (manaPickupPrefab != null)
        {
            Vector3 offset = new Vector3(-0.4f, 0.3f, 0);
            Instantiate(manaPickupPrefab, transform.position + offset, Quaternion.identity);
        }

        // Chia ra quái nhỏ (nếu có)
        if (smallEnemyPrefab != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 spawnPos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                Instantiate(smallEnemyPrefab, spawnPos, Quaternion.identity);
            }
        }

        // Xoá enemy sau 1.5s
        Destroy(gameObject, 1.5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hit"))
        {
            TakeDamage(25);
        }
    }
}
