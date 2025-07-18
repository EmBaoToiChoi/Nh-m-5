using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    public GameObject goldPrefab;
    public GameObject smallEnemyPrefab;

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
        {
            animator.SetTrigger("Hurt");
        }

        if (hurtClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(hurtClip);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (deathClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathClip);
        }

        // R?t vàng khi ch?t
        if (goldPrefab != null)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }

        // N?u là Boss th? sinh ra 3 quái nh?
        if (gameObject.CompareTag("Boss"))
        {
            if (smallEnemyPrefab != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector3 pos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                    Instantiate(smallEnemyPrefab, pos, Quaternion.identity);
                }
            }
        }

        Destroy(gameObject, 1.5f); // Delay ð? âm thanh phát xong
    }
}
