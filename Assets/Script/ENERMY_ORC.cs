using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ENERMY_ORC : MonoBehaviour
{
    public Transform enermy, player;
    [SerializeField] private GameObject coinPrefab;

    [SerializeField] private GameObject xpPrefab;


    [SerializeField] private GameObject healthItemPrefab;


    [Header("Âm thanh và animation")]
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioSource source2;
    [SerializeField] private Animator nie;

    [Header("UI máu")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Image healthFill; // Gán image fill trong inspector

    private GameObject healthBarUI;
    private Transform mainCam;

    private float speed = 2f;
    private float PVipHien = 10f;
    private float maxHealth = 20f;
    private float currentHealth;

    private bool isChasing = false;

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
        float khoangCachPlayer = Vector2.Distance(enermy.position, player.position);
        isChasing = khoangCachPlayer < PVipHien;

        if (isChasing)
        {
            dichuyentoiPlayer(player.position);
        }

        // Cập nhật vị trí thanh máu
        if (healthBarUI != null)
        {
            healthBarUI.transform.position = enermy.position + Vector3.up * 1.5f;
            healthBarUI.transform.rotation = Quaternion.identity; // Không xoay trong 2D
        }
    }

    void dichuyentoiPlayer(Vector3 target)
    {
        Vector3 direction = (target - enermy.position).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        // Lật mặt
        if (direction.x > 0)
            enermy.localScale = new Vector3(10, 10, 10);
        if (direction.x < 0)
            enermy.localScale = new Vector3(-10, 10, 10);
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
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hit"))
        {
            float damage = Random.Range(1f, 6f); // 1–5
            TakeDamage(damage);
        }
        else if (other.CompareTag("Bullet"))
        {
            float damage = Random.Range(10f, 16f); // 10–15
            TakeDamage(damage);
        }
        else if (other.CompareTag("Bow"))
        {
            float damage = Random.Range(5f, 11f); // 5–10
            TakeDamage(damage);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            source2.PlayOneShot(hit);
            nie.SetBool("danh", true);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            source2.PlayOneShot(hit);
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
