using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quy3 : MonoBehaviour
{
    [SerializeField] private float fleeHealthThreshold = 10f;

    public Transform enermy;
    public Transform player1;
    public Transform player2;
    public Transform player3;
    private Transform targetPlayer;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject xpPrefab;
    [SerializeField] private GameObject healthItemPrefab;

    [Header("Animation")]
    [SerializeField] private Animator nie;

    [Header("UI Máu")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Image healthFill;
    private GameObject healthBarUI;
    private Transform mainCam;

    private bool isChasing = false;
    private float speed = 1f;
    private float PVipHien = 20f;

    private float maxHealth = 600f;
    private float currentHealth;

    [Header("Triệu hồi quái")]
    [SerializeField] private GameObject[] extraEnemyPrefabs;
    [SerializeField] private float spawnDistance = 2f;
    [SerializeField] private float summonCooldown = 15f; // 15s triệu hồi
    private float summonTimer = 0f;

    [Header("Skill Cầu Lửa")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireballSpeed = 5f;
    [SerializeField] private float fireCooldown = 5f; // 5s bắn
    private float fireTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;

        // Xác định player
        if (player1 != null && player1.gameObject.activeInHierarchy)
            targetPlayer = player1;
        else if (player2 != null && player2.gameObject.activeInHierarchy)
            targetPlayer = player2;
        else if (player3 != null && player3.gameObject.activeInHierarchy)
            targetPlayer = player3;
        else
            Debug.LogWarning("Không có player nào đang active!");

        // Tạo thanh máu
        healthBarUI = Instantiate(healthBarPrefab, enermy.position + Vector3.up * 1.5f, Quaternion.identity);
        healthBarUI.transform.SetParent(null);
        mainCam = Camera.main.transform;
    }

    void Update()
    {
        if (this == null || enermy == null || targetPlayer == null) return;

        float khoangCachPlayer = Vector2.Distance(enermy.position, targetPlayer.position);
        isChasing = khoangCachPlayer < PVipHien;

        if (currentHealth <= fleeHealthThreshold)
        {
            ChayKhoiPlayer(targetPlayer.position);
        }
        else if (isChasing)
        {
            // Luôn di chuyển về phía player
            dichuyentoiPlayer(targetPlayer.position);

            // Đếm thời gian skill cầu lửa
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireCooldown)
            {
                BanCauLua();
                fireTimer = 0f;
            }

            // Đếm thời gian skill triệu hồi
            summonTimer += Time.deltaTime;
            if (summonTimer >= summonCooldown)
            {
                TrieuHoiQuai();
                summonTimer = 0f;
            }
        }

        // Cập nhật thanh máu
        if (healthBarUI != null && enermy != null)
        {
            healthBarUI.transform.position = enermy.position + Vector3.up * 1.5f;
            healthBarUI.transform.rotation = Quaternion.identity;
        }
    }

    void dichuyentoiPlayer(Vector3 target)
    {
        Vector3 direction = (target - enermy.position).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        if (direction.x > 0)
            enermy.localScale = new Vector3(5, 5, 5);
        else if (direction.x < 0)
            enermy.localScale = new Vector3(-5, 5, 5);
    }

    void ChayKhoiPlayer(Vector3 target)
    {
        Vector3 direction = (enermy.position - target).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        if (direction.x > 0)
            enermy.localScale = new Vector3(5, 5, 5);
        else if (direction.x < 0)
            enermy.localScale = new Vector3(-5, 5, 5);
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
            if (nie != null) nie.SetTrigger("die");
            this.enabled = false;
            SpawnDrops();
            Destroy(healthBarUI);
            Destroy(gameObject, 1f);
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

    void SpawnDrops()
    {
        Vector3 basePosition = enermy.position;
        if (coinPrefab != null)
            Instantiate(coinPrefab, basePosition + new Vector3(-0.3f, 0, 0), Quaternion.identity);
        if (xpPrefab != null)
            Instantiate(xpPrefab, basePosition, Quaternion.identity);
        if (healthItemPrefab != null)
            Instantiate(healthItemPrefab, basePosition + new Vector3(0.3f, 0, 0), Quaternion.identity);
    }

    void TrieuHoiQuai()
    {
        if (nie != null) nie.SetTrigger("trieuhoi");

        if (extraEnemyPrefabs == null || extraEnemyPrefabs.Length == 0) return;
        for (int i = 0; i < 3; i++) // luôn spawn 3 con
        {
            GameObject prefab = extraEnemyPrefabs[Random.Range(0, extraEnemyPrefabs.Length)];
            if (prefab != null)
            {
                float angle = (360f / 3) * i;
                Vector3 spawnPos = enermy.position + new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad),
                    0
                ) * spawnDistance;
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }
    }

    void BanCauLua()
    {
        if (nie != null) nie.SetTrigger("lua");

        if (fireballPrefab != null && firePoint != null && targetPlayer != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (targetPlayer.position - firePoint.position).normalized;
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = direction * fireballSpeed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fireball.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
