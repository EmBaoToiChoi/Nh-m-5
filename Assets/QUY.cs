using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QUY : MonoBehaviour
{
    [SerializeField] private float fleeHealthThreshold = 10f;
    public Transform enermy, player;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject xpPrefab;

    [SerializeField] private GameObject healthItemPrefab;


    [Header("Animation")]
    [SerializeField] private Animator nie;

    [Header("UI Máu")]
    [SerializeField] private GameObject healthBarPrefab; // Prefab thanh máu
    [SerializeField] private Image healthFill;           // Gán trong Inspector

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

        // Spawn thanh máu
        healthBarUI = Instantiate(healthBarPrefab, enermy.position + Vector3.up * 1.5f, Quaternion.identity);
        healthBarUI.transform.SetParent(null);

        mainCam = Camera.main.transform;
    }

    void Update()
    {
        // Nếu object đã bị phá hủy hoặc enermy không còn, thì không làm gì
        if (this == null || enermy == null || player == null) return;

        float khoangCachPlayer = Vector2.Distance(enermy.position, player.position);
        isChasing = khoangCachPlayer < PVipHien;

        if (currentHealth <= fleeHealthThreshold)
        {
            ChayKhoiPlayer(player.position);
        }
        else if (isChasing)
        {
            dichuyentoiPlayer(player.position);
        }

        // Cập nhật vị trí thanh máu nếu mọi thứ còn tồn tại
        if (healthBarUI != null && enermy != null)
        {
            try
            {
                healthBarUI.transform.position = enermy.position + Vector3.up * 1.5f;
                healthBarUI.transform.rotation = Quaternion.identity;
            }
            catch (MissingReferenceException)
            {
                // Nếu enermy đã bị huỷ giữa Update, ta đảm bảo không bị lỗi
                Destroy(healthBarUI);
            }
        }
    }





    void dichuyentoiPlayer(Vector3 target)
    {
        Vector3 direction = (target - enermy.position).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        // Lật hướng enemy
        if (direction.x > 0)
            enermy.localScale = new Vector3(-3,3,3);
        if (direction.x < 0)
            enermy.localScale = new Vector3(3,3,3);
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

            enabled = false; // Dừng Update() để tránh lỗi
        }
        if (healthFill != null && enermy != null)
        {
            DamageTextManager.Instance.ShowDamage(enermy.position, damage);
            healthFill.fillAmount = currentHealth / maxHealth;
        }


    }


    void ChayKhoiPlayer(Vector3 target)
    {
        Vector3 direction = (enermy.position - target).normalized; // Ngược hướng với player
        enermy.Translate(direction * speed * Time.deltaTime);

        // Lật hướng enemy
        if (direction.x > 0)
            enermy.localScale = new Vector3(-3,3,3);
        else if (direction.x < 0)
            enermy.localScale = new Vector3(3,3,3);
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

        // Spawn coin slightly to the left
        if (coinPrefab != null)
            Instantiate(coinPrefab, basePosition + new Vector3(-0.3f, 0, 0), Quaternion.identity);

        // Spawn XP slightly to the center
        if (xpPrefab != null)
            Instantiate(xpPrefab, basePosition + new Vector3(0f, 0, 0), Quaternion.identity);

        // Spawn health item slightly to the right
        if (healthItemPrefab != null)
            Instantiate(healthItemPrefab, basePosition + new Vector3(0.3f, 0, 0), Quaternion.identity);
    }


}
