using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doi : MonoBehaviour
{
    private enum State { Chase, Attack, Flee, Return }
    private State currentState = State.Chase;

    [SerializeField] private float fleeDuration = 1.5f;
    [SerializeField] private float returnDelay = 0.5f;
    private float fleeTimer = 0f;
    private Vector3 fleeDirection;

    [SerializeField] private float fleeHealthThreshold = 10f;

    [Header("References")]
    public Transform enermy;

    // Thêm 3 player ở đây
    public Transform player1;
    public Transform player2;
    public Transform player3;

    private Transform player; // player được chọn

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
    [SerializeField] private GameObject[] extraEnemyPrefabs; // Thêm mảng 3 prefab quái khác
    [SerializeField] private float spawnDistance = 2f;
    [SerializeField] private float attackRange = 1.2f;


    void Start()
    {
        currentHealth = maxHealth;

        // Xác định player được chọn dựa vào SetActive(true)
        if (player1 != null && player1.gameObject.activeSelf)
            player = player1;
        else if (player2 != null && player2.gameObject.activeSelf)
            player = player2;
        else if (player3 != null && player3.gameObject.activeSelf)
            player = player3;
        else
            Debug.LogWarning("Không tìm thấy player nào đang hoạt động!");

        // Spawn thanh máu
        healthBarUI = Instantiate(healthBarPrefab, enermy.position + Vector3.up * 1.5f, Quaternion.identity);
        healthBarUI.transform.SetParent(null);
        mainCam = Camera.main.transform;
    }

    void Update()
    {
        if (this == null || enermy == null || player == null) return;

        switch (currentState)
        {
            case State.Chase:
                float distance = Vector2.Distance(enermy.position, player.position);
                isChasing = distance < PVipHien;

                if (currentHealth <= fleeHealthThreshold)
                {
                    ChayKhoiPlayer(player.position);
                }
                else if (isChasing)
                {
                    // Chỉ di chuyển nếu chưa tới khoảng tấn công
                    if (distance > attackRange)
                    {
                        dichuyentoiPlayer(player.position);
                        nie.SetBool("danh", false);
                    }
                    else
                    {
                        nie.SetBool("danh", true); // Đứng tấn công
                    }
                }
                break;

            case State.Flee:
                fleeTimer -= Time.deltaTime;
                enermy.Translate(fleeDirection * speed * Time.deltaTime);
                if (fleeTimer <= 0f)
                {
                    StartCoroutine(DelayReturn());
                }
                break;

            case State.Return:
                if (Vector2.Distance(enermy.position, player.position) > attackRange)
                    dichuyentoiPlayer(player.position);
                break;
        }

        // Cập nhật UI thanh máu
        if (healthBarUI != null && enermy != null)
        {
            healthBarUI.transform.position = enermy.position + Vector3.up * 1.5f;
            healthBarUI.transform.rotation = Quaternion.identity;
        }
    }

    IEnumerator DelayReturn()
    {
        yield return new WaitForSeconds(returnDelay);
        currentState = State.Return;
    }

    void StartFlee()
    {
        currentState = State.Flee;
        fleeTimer = fleeDuration;
        fleeDirection = (enermy.position - player.position).normalized;

        if (fleeDirection.x > 0)
            enermy.localScale = new Vector3(5, 5, 5);
        else if (fleeDirection.x < 0)
            enermy.localScale = new Vector3(-5, 5, 5);
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
            // Kích hoạt animation chết
            if (nie != null)
                nie.SetTrigger("die");

            // Ngừng di chuyển, tránh bug animation
            this.enabled = false;

            // Spawn item rơi
            SpawnDrops();

            // Spawn thêm quái khác
            SpawnExtraEnemies();

            // Xóa thanh máu
            Destroy(healthBarUI);

            // Hủy object sau khi animation kết thúc (1 giây)
            Destroy(gameObject, 1f);
        }
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
            if (currentState == State.Chase || currentState == State.Return)
                StartFlee();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            nie.SetBool("danh", true);
            if (currentState == State.Chase || currentState == State.Return)
                StartFlee();
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
    void SpawnExtraEnemies()
    {
        if (extraEnemyPrefabs == null || extraEnemyPrefabs.Length == 0) return;

        for (int i = 0; i < extraEnemyPrefabs.Length; i++)
        {
            if (extraEnemyPrefabs[i] != null)
            {
                // Xoay vòng vị trí spawn cách đều nhau
                float angle = (360f / extraEnemyPrefabs.Length) * i;
                Vector3 spawnPos = enermy.position + new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad),
                    0
                ) * spawnDistance;

                Instantiate(extraEnemyPrefabs[i], spawnPos, Quaternion.identity);
            }
        }
    }

}
