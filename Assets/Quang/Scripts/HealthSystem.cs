using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Cài đặt máu")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("UI Máu")]
    public GameObject healthBarPrefab;
    public Image healthFill;
    private GameObject healthBarUI;
    private Transform mainCam;

    [Header("Prefab Rơi Vật Phẩm")]
    public GameObject goldPrefab;
    public GameObject smallEnemyPrefab;
    public GameObject healthPickupPrefab;
    public GameObject xpPrefab;

    [Header("Âm thanh")]
    public AudioClip hurtClip;
    public AudioClip deathClip;
    private AudioSource audioSource;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Spawn UI máu nếu có prefab
        if (healthBarPrefab != null)
        {
            healthBarUI = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            healthBarUI.transform.SetParent(null);
            mainCam = Camera.main.transform;
        }
    }

    void Update()
    {
        // Cập nhật vị trí UI máu theo boss
        if (healthBarUI != null)
        {
            healthBarUI.transform.position = transform.position + Vector3.up * 2f;
            healthBarUI.transform.rotation = Quaternion.identity;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Hit") || collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Bow"))
        {
            int damage = 25; // Hoặc đổi theo weapon
            TakeDamage(damage);
        }
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (hurtClip != null && audioSource != null)
            audioSource.PlayOneShot(hurtClip);

        // Hiển thị damage text
        DamageTextManager.Instance.ShowDamage(transform.position, amount);

        // Cập nhật UI máu
        if (healthFill != null)
            healthFill.fillAmount = (float)currentHealth / maxHealth;

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
            Instantiate(goldPrefab, transform.position, Quaternion.identity);

        // Rơi máu
        if (healthPickupPrefab != null)
            Instantiate(healthPickupPrefab, transform.position + new Vector3(0.4f, 0.3f, 0), Quaternion.identity);

        // Rơi XP
        if (xpPrefab != null)
            Instantiate(xpPrefab, transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);

        // Sinh 3 quái nhỏ
        if (smallEnemyPrefab != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 pos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                Instantiate(smallEnemyPrefab, pos, Quaternion.identity);
            }
        }

        if (healthBarUI != null)
            Destroy(healthBarUI);

        Destroy(gameObject, 1.5f); // chờ animation xong rồi huỷ
    }
}
