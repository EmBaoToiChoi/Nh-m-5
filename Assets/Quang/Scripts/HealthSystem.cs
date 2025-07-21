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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Hit")) // Đạn của player
        {
            currentHealth -= 25;

            Debug.Log($"{gameObject.name} bị trúng đòn! HP còn: {currentHealth}");

            if (animator != null)
                animator.SetTrigger("Hurt");

            if (hurtClip != null && audioSource != null)
                audioSource.PlayOneShot(hurtClip);

            if (currentHealth <= 0)
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

                // Rơi máu
                if (healthPickupPrefab != null)
                {
                    Vector3 offset = new Vector3(0.4f, 0.3f, 0);
                    Instantiate(healthPickupPrefab, transform.position + offset, Quaternion.identity);
                }

                // 👇 Sinh 3 quái nhỏ khi chết (áp dụng cho mọi enemy)
                if (smallEnemyPrefab != null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector3 spawnPos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                        Instantiate(smallEnemyPrefab, spawnPos, Quaternion.identity);
                    }
                }

                // Xoá object sau khi chết
                Destroy(gameObject, 1.5f);
            }
        }
    }
}
