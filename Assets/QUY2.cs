using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QUY2 : MonoBehaviour
{
    [Header("References")]
    public Transform enermy, player;
    public Animator nie;
    public GameObject fireballPrefab;
    public Transform firePoint;

    [Header("Drop Prefabs")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject xpPrefab;
    [SerializeField] private GameObject healthItemPrefab;

    [Header("UI Máu")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Image healthFill;

    [Header("Enemy Settings")]
    [SerializeField] private float fleeHealthThreshold = 10f;
    [SerializeField] private float maxHealth = 80f;
    [SerializeField] private float PVipHien = 10f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float fireRate = 1f;

    private GameObject healthBarUI;
    private Transform mainCam;
    private float currentHealth;
    private bool isChasing = false;
    private Coroutine fireCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarUI = Instantiate(healthBarPrefab, enermy.position + Vector3.up * 1.5f, Quaternion.identity);
        healthBarUI.transform.SetParent(null);
        mainCam = Camera.main.transform;
    }

    void Update()
    {
        if (enermy == null || player == null) return;

        float distance = Vector2.Distance(enermy.position, player.position);
        isChasing = distance < PVipHien;

        if (currentHealth <= fleeHealthThreshold)
        {
            ChayKhoiPlayer(player.position);
            StopFireball();
        }
        else if (isChasing)
        {
            dichuyentoiPlayer(player.position);
            nie.SetBool("danh", true);
            if (fireCoroutine == null)
                fireCoroutine = StartCoroutine(ShootFireballsContinuously());
        }
        else
        {
            nie.SetBool("danh", false);
            StopFireball();
        }

        if (healthBarUI != null)
        {
            try
            {
                healthBarUI.transform.position = enermy.position + Vector3.up * 1.5f;
                healthBarUI.transform.rotation = Quaternion.identity;
            }
            catch (MissingReferenceException)
            {
                Destroy(healthBarUI);
            }
        }
    }

    void dichuyentoiPlayer(Vector3 target)
    {
        Vector3 direction = (target - enermy.position).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        if (direction.x > 0)
            enermy.localScale = new Vector3(-3, 3, 3);
        else if (direction.x < 0)
            enermy.localScale = new Vector3(3, 3, 3);
    }

    void ChayKhoiPlayer(Vector3 target)
    {
        Vector3 direction = (enermy.position - target).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        if (direction.x > 0)
            enermy.localScale = new Vector3(-3, 3, 3);
        else if (direction.x < 0)
            enermy.localScale = new Vector3(3, 3, 3);
    }

    IEnumerator ShootFireballsContinuously()
    {
        while (true)
        {
            ShootFireball();
            yield return new WaitForSeconds(fireRate);
        }
    }

    void ShootFireball()
    {
        if (fireballPrefab != null && firePoint != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            Vector2 dir = (player.position - firePoint.position).normalized;
            fireball.GetComponent<Rigidbody2D>().velocity = dir * 5f;
        }
    }

    void StopFireball()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthFill != null)
        {
            DamageTextManager.Instance.ShowDamage(enermy.position, damage);
            healthFill.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            DamageTextManager.Instance.ShowDamage(enermy.position, damage);
            SpawnDrops();
            Destroy(healthBarUI);
            Destroy(gameObject);
            StopFireball();
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hit"))
            TakeDamage(Random.Range(1f, 6f) + GlobalData.damageBonus);
        else if (other.CompareTag("Bullet"))
            TakeDamage(Random.Range(10f, 16f) + GlobalData.damageBonus);
        else if (other.CompareTag("Bow"))
            TakeDamage(Random.Range(5f, 11f) + GlobalData.damageBonus);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
            nie.SetBool("danh", true);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
            nie.SetBool("danh", true);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
            nie.SetBool("danh", false);
    }

    void SpawnDrops()
    {
        Vector3 pos = enermy.position;
        if (coinPrefab != null) Instantiate(coinPrefab, pos + new Vector3(-0.3f, 0, 0), Quaternion.identity);
        if (xpPrefab != null) Instantiate(xpPrefab, pos + new Vector3(0f, 0, 0), Quaternion.identity);
        if (healthItemPrefab != null) Instantiate(healthItemPrefab, pos + new Vector3(0.3f, 0, 0), Quaternion.identity);
    }
}
