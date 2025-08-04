using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfScript : MonoBehaviour
{
    public Transform player; // Gán player từ Inspector
    public float patrolRange = 5f;
    public float detectionRange = 3f;
    public float speed = 2f;
    public int maxHealth = 3; // Thêm máu cho enemy
    public GameObject goldPrefab; // Prefab vàng
    public GameObject smallEnemyPrefab; // Prefab quái nhỏ
    public GameObject attackEffectPrefab; // Prefab hiệu ứng đánh
    public Animator ani; // Thêm Animation nếu cần
    private int currentHealth;
    private Vector3 startPoint;
    private Vector3 patrolTarget;
    private bool isAttacking = false;
    public GameObject healthPrefab;
    public EnemyHealthBar healthBar;
    void Start()
    {
        startPoint = transform.position;
        SetNewPatrolTarget();
        currentHealth = maxHealth;
    }
    void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player1");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        healthBar = FindObjectOfType<EnemyHealthBar>();
    }

    void Update()
    {
        if (player == null) return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 targetPosition = patrolTarget;
        if (distanceToPlayer <= detectionRange)
        {
            // Đuổi theo player
            targetPosition = player.position;
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Chạy hiệu ứng đánh khi player vào vùng tuần tra
            if (!isAttacking)
            {
                StartCoroutine(PlayAttackEffect());
            }
        }
        else
        {
            Patrol();
            targetPosition = patrolTarget;
        }
         if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);
        ani?.Play("DwarfWalk");
        if (Vector3.Distance(transform.position, patrolTarget) < 0.2f)
        {
            SetNewPatrolTarget();
        }
    }

    void SetNewPatrolTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRange;
        patrolTarget = startPoint + new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Rơi vàng
        if (goldPrefab != null)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }
        if (healthPrefab != null)
        {
        Instantiate(healthPrefab, transform.position, Quaternion.identity);
        }
        // Nếu là quái lớn thì sinh ra 3 quái nhỏ
        if (smallEnemyPrefab != null && transform.localScale == Vector3.one)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject smallEnemy = Instantiate(smallEnemyPrefab, transform.position + Random.insideUnitSphere, Quaternion.identity);
                smallEnemy.transform.localScale = transform.localScale * 0.5f; // Nhỏ hơn enemy ban đầu
            }
        }

        Destroy(gameObject);
    }

    // Hiệu ứng đánh
    IEnumerator PlayAttackEffect()
    {
        isAttacking = true;
        ani?.Play("DwarfAttack");
        yield return new WaitForSeconds(0.5f); // Thời gian hiệu ứng
        if (attackEffectPrefab != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f); // Xoá hiệu ứng sau 0.5s
        }
        yield return new WaitForSeconds(1f); // Thời gian giữa các lần đánh
        isAttacking = false;
    }
}
