using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Prefab Drop")]
    public GameObject goldPrefab;
    public GameObject smallEnemyPrefab;
    public GameObject healthPickupPrefab;

    private Animator animator;
    private AudioSource audioSource;

    [Header("Âm thanh")]
    public AudioClip hurtClip;
    public AudioClip deathClip;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"{gameObject.name} b? trúng ð?n! HP c?n: {currentHealth}");

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (hurtClip != null && audioSource != null)
            audioSource.PlayOneShot(hurtClip);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        if (deathClip != null && audioSource != null)
            audioSource.PlayOneShot(deathClip);

        // R?t vàng ngay v? trí ch?t
        if (goldPrefab != null)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }

        // R?t máu ? v? trí l?ch nh? ð? không trùng vàng
        if (healthPickupPrefab != null)
        {
            Vector3 offset = new Vector3(0.4f, 0.3f, 0); // l?ch nh? kh?i vàng
            Instantiate(healthPickupPrefab, transform.position + offset, Quaternion.identity);
        }

        // N?u là Boss ? chia ra 3 enemy nh?
        if (CompareTag("Boss") && smallEnemyPrefab != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 spawnPos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                Instantiate(smallEnemyPrefab, spawnPos, Quaternion.identity);
            }
        }

        Destroy(gameObject, 1.5f); // Delay ð? anim + âm thanh phát xong
    }
}
