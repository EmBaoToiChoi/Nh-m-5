using UnityEngine;
using UnityEngine.UI;

public class Healthsystem1 : MonoBehaviour
{
    [Header("C�i �?t m�u")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("UI M�u")]
    public GameObject healthBarPrefab;
    public Image healthFill;
    private GameObject healthBarUI;

    [Header("Prefab R�i V?t Ph?m")]
    public GameObject goldPrefab;
    public GameObject healthPickupPrefab;
    public GameObject xpPrefab;

    [Header("Tri?u h?i khi m�u th?p")]
    public GameObject lowHealthMonsterPrefab1;
    public GameObject lowHealthMonsterPrefab2;
    public GameObject lowHealthMonsterPrefab3;
    private bool hasLowHealthSummoned = false;

    [Header("�m thanh")]
    public AudioClip hurtClip;
    public AudioClip deathClip;
    private AudioSource audioSource;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Spawn UI m�u n?u c� prefab
        if (healthBarPrefab != null)
        {
            healthBarUI = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            healthBarUI.transform.SetParent(null);
        }
    }

    void Update()
    {
        // C?p nh?t v? tr� UI m�u theo boss
        if (healthBarUI != null)
        {
            healthBarUI.transform.position = transform.position + Vector3.up * 2f;
            healthBarUI.transform.rotation = Quaternion.identity;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hit"))
        {
            float damage = Random.Range(1f, 6f);
            TakeDamage(damage + GlobalData.damageBonus);
        }
        else if (other.CompareTag("Bullet"))
        {
            float damage = Random.Range(10f, 16f);
            TakeDamage(damage + GlobalData.damageBonus);
        }
        else if (other.CompareTag("Bow"))
        {
            float damage = Random.Range(5f, 11f);
            TakeDamage(damage + GlobalData.damageBonus);
        }
    }



    void TakeDamage(float amount)
    {
        currentHealth -= (int)amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Animation b? th��ng
        if (animator != null)
            animator.SetTrigger("Hurt");

        // �m thanh b? th��ng
        if (hurtClip != null && audioSource != null)
            audioSource.PlayOneShot(hurtClip);

        // Hi?n Damage Text (tr�nh l?i null)
        if (DamageTextManager.Instance != null && DamageTextManager.Instance.worldCanvas != null)
        {
            DamageTextManager.Instance.ShowDamage(transform.position, amount);
        }

        // C?p nh?t UI m�u
        if (healthFill != null)
            healthFill.fillAmount = (float)currentHealth / maxHealth;

        // Tri?u h?i khi m�u <= 50% v� ch�a tri?u h?i
        if (!hasLowHealthSummoned && currentHealth <= maxHealth * 0.5f)
        {
            SummonLowHealthMonsters();
            hasLowHealthSummoned = true;
        }

        // Ch?t
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void SummonLowHealthMonsters()
    {
        Vector3 pos1 = transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
        Vector3 pos2 = transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
        Vector3 pos3 = transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);

        if (lowHealthMonsterPrefab1 != null)
        {
            GameObject e1 = Instantiate(lowHealthMonsterPrefab1, pos1, Quaternion.identity);
            e1.transform.localScale = lowHealthMonsterPrefab1.transform.localScale;
        }

        if (lowHealthMonsterPrefab2 != null)
        {
            GameObject e2 = Instantiate(lowHealthMonsterPrefab2, pos2, Quaternion.identity);
            e2.transform.localScale = lowHealthMonsterPrefab2.transform.localScale;
        }

        if (lowHealthMonsterPrefab3 != null)
        {
            GameObject e3 = Instantiate(lowHealthMonsterPrefab3, pos3, Quaternion.identity);
            e3.transform.localScale = lowHealthMonsterPrefab3.transform.localScale;
        }
    }

    void Die()
    {
        isDead = true;

        // Animation ch?t
        if (animator != null)
            animator.SetTrigger("Die");

        // �m thanh ch?t
        if (deathClip != null && audioSource != null)
            audioSource.PlayOneShot(deathClip);

        // R�i v�ng
        if (goldPrefab != null)
            Instantiate(goldPrefab, transform.position, Quaternion.identity);

        // R�i m�u
        if (healthPickupPrefab != null)
            Instantiate(healthPickupPrefab, transform.position + new Vector3(0.4f, 0.3f, 0), Quaternion.identity);

        // R�i XP
        if (xpPrefab != null)
            Instantiate(xpPrefab, transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);

        // Xo� thanh m�u UI
        if (healthBarUI != null)
            Destroy(healthBarUI);

        Destroy(gameObject, 1.5f); // ch? animation xong r?i hu?
    }
}
