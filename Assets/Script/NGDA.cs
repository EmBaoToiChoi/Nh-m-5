using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NGDA : MonoBehaviour
{
    private float attackCooldown = 0.5f;
    private float attackTimer = 0f;

    private bool hasTeleportedToPlayer = false;
    [SerializeField] private float fleeHealthThreshold = 10f;

    public Transform enermy;
    public Transform player1;
    public Transform player2;
    public Transform player3;

    private Transform targetPlayer; // Người chơi mà enemy sẽ đuổi theo

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
    private float speed = 3f;
    private float PVipHien = 5f;

    private float maxHealth = 200f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        healthBarUI = Instantiate(healthBarPrefab, enermy.position + Vector3.up * 1.5f, Quaternion.identity);
        healthBarUI.transform.SetParent(null);

        mainCam = Camera.main.transform;
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        UpdateTargetPlayer(); // Cập nhật player cần đuổi theo

        if (targetPlayer == null || enermy == null) return;

        float distance = Vector2.Distance(enermy.position, targetPlayer.position);
        isChasing = distance < PVipHien;

        if (currentHealth <= fleeHealthThreshold)
        {
            ChayKhoiPlayer(targetPlayer.position);
        }
        else if (isChasing)
        {
            dichuyentoiPlayer(targetPlayer.position);

            if (!hasTeleportedToPlayer)
            {
                TeleportToPlayer();
                hasTeleportedToPlayer = true;
            }
        }

        if (healthBarUI != null && enermy != null)
        {
            healthBarUI.transform.position = enermy.position + Vector3.up * 1.5f;
            healthBarUI.transform.rotation = Quaternion.identity;
        }
    }

    void UpdateTargetPlayer()
    {
        // Ưu tiên theo thứ tự: player1 → player2 → player3
        if (player1 != null && player1.gameObject.activeInHierarchy)
        {
            targetPlayer = player1;
        }
        else if (player2 != null && player2.gameObject.activeInHierarchy)
        {
            targetPlayer = player2;
        }
        else if (player3 != null && player3.gameObject.activeInHierarchy)
        {
            targetPlayer = player3;
        }
        else
        {
            targetPlayer = null; // Không có ai để đuổi
        }
    }

    void TeleportToPlayer()
    {
        if (enermy == null || targetPlayer == null) return;

        Vector3 offset = (enermy.position - targetPlayer.position).normalized * 1.0f;
        Vector3 teleportPosition = targetPlayer.position + offset;

        enermy.position = teleportPosition;

        Debug.Log("Enemy đã teleport tới gần player!");
    }

    void dichuyentoiPlayer(Vector3 target)
    {
        Vector3 direction = (target - enermy.position).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        if (direction.x > 0)
            enermy.localScale = new Vector3(3, 3, 3);
        if (direction.x < 0)
            enermy.localScale = new Vector3(-3, 3, 3);
    }

    void ChayKhoiPlayer(Vector3 target)
    {
        Vector3 direction = (enermy.position - target).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        if (direction.x > 0)
            enermy.localScale = new Vector3(3, 3, 3);
        else if (direction.x < 0)
            enermy.localScale = new Vector3(-3, 3, 3);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            nie.SetBool("danh", true);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") )
        {
            nie.SetBool("danh", true);

            if (attackTimer <= 0f)
            {
                float damage = Random.Range(4f, 8f);
                collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                attackTimer = attackCooldown;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            nie.SetBool("danh", false);
        }
    }
}
