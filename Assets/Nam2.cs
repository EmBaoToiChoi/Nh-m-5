using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nam2 : MonoBehaviour
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
    private float speed = 4f;
    private float PVipHien = 15f;

    private float maxHealth = 80f;
    private float currentHealth;

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
            dichuyentoiPlayer(targetPlayer.position);
        }

        // Cập nhật vị trí thanh máu
        if (healthBarUI != null && enermy != null)
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
            DamageTextManager.Instance.ShowDamage(enermy.position, damage);
            SpawnDrops();

            Destroy(healthBarUI);
            Destroy(gameObject);
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
        if (collision.gameObject.CompareTag("Player1"))
        {
            nie.SetBool("danh", true);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            nie.SetBool("danh", true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            nie.SetBool("danh", false);
        }
    }

    void SpawnDrops()
    {
        Vector3 basePosition = enermy.position;

        if (coinPrefab != null)
            Instantiate(coinPrefab, basePosition + new Vector3(-0.3f, 0, 0), Quaternion.identity);

        if (xpPrefab != null)
            Instantiate(xpPrefab, basePosition + new Vector3(0f, 0, 0), Quaternion.identity);

        if (healthItemPrefab != null)
            Instantiate(healthItemPrefab, basePosition + new Vector3(0.3f, 0, 0), Quaternion.identity);
    }
}
