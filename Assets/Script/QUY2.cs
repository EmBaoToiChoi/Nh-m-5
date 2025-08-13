using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QUY2 : MonoBehaviour
{
    [Header("References")]
    public Transform enermy;
    public Transform player1;
    public Transform player2;
    public Transform player3;

    private Transform targetPlayer;

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
    [SerializeField] private float maxHealth = 200f;
    [SerializeField] private float PVipHien = 5f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float fireRate = 1f;

    private GameObject healthBarUI;
    private Transform mainCam;
    private float currentHealth;
    private bool isChasing = false;
    private Coroutine fireCoroutine;

    void Start()
    {
        currentHealth = maxHealth;

        // Xác định player đang active
        if (player1 != null && player1.gameObject.activeInHierarchy)
            targetPlayer = player1;
        else if (player2 != null && player2.gameObject.activeInHierarchy)
            targetPlayer = player2;
        else if (player3 != null && player3.gameObject.activeInHierarchy)
            targetPlayer = player3;
        else
            Debug.LogWarning("Không có player nào đang active!");

        healthBarUI = Instantiate(healthBarPrefab, enermy.position + Vector3.up * 1.5f, Quaternion.identity);
        healthBarUI.transform.SetParent(null);
        mainCam = Camera.main.transform;
    }

    void Update()
    {
        if (enermy == null || targetPlayer == null) return;

        float distance = Vector2.Distance(enermy.position, targetPlayer.position);
        isChasing = distance < PVipHien;

        if (currentHealth <= fleeHealthThreshold)
        {
            ChayKhoiPlayer(targetPlayer.position);
            StopFireball();
        }
        else if (isChasing)
        {
            dichuyentoiPlayer(targetPlayer.position);
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
        if (fireballPrefab != null && firePoint != null && targetPlayer != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            Vector2 dir = (targetPlayer.position - firePoint.position).normalized;
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
        float damage = 0f;

        if (other.CompareTag("Hit"))
            damage = Random.Range(1f, 6f);
        else if (other.CompareTag("Bullet"))
            damage = Random.Range(10f, 16f);
        else if (other.CompareTag("Bow"))
            damage = Random.Range(5f, 11f);

        if (damage > 0)
            TakeDamage(damage + GlobalData.damageBonus);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsTargetPlayer(collision.gameObject))
            nie.SetBool("danh", true);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (IsTargetPlayer(collision.gameObject))
            nie.SetBool("danh", true);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (IsTargetPlayer(collision.gameObject))
            nie.SetBool("danh", false);
    }

    bool IsTargetPlayer(GameObject obj)
    {
        return obj == player1?.gameObject || obj == player2?.gameObject || obj == player3?.gameObject;
    }

    void SpawnDrops()
    {
        Vector3 pos = enermy.position;
        if (coinPrefab != null) Instantiate(coinPrefab, pos + new Vector3(-0.3f, 0, 0), Quaternion.identity);
        if (xpPrefab != null) Instantiate(xpPrefab, pos + new Vector3(0f, 0, 0), Quaternion.identity);
        if (healthItemPrefab != null) Instantiate(healthItemPrefab, pos + new Vector3(0.3f, 0, 0), Quaternion.identity);
    }
}
