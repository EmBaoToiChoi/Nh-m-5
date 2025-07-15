using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Script : MonoBehaviour
{
    public Transform player; // Gán player từ Inspector
    public float patrolRange = 5f;
    public float detectionRange = 3f;
    public float speed = 2f;
    public int maxHealth = 3; // Thêm máu cho enemy
    public GameObject goldPrefab; // Prefab vàng
    public GameObject smallEnemyPrefab; // Prefab quái nhỏ
    public GameObject attackEffectPrefab; // Prefab hiệu ứng đánh

    private int currentHealth;
    private Vector3 startPoint;
    private Vector3 patrolTarget;
    private bool isAttacking = false;

    void Start()
    {
        startPoint = transform.position;
        SetNewPatrolTarget();
        currentHealth = maxHealth;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Đuổi theo player
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
        }
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);

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
        if (attackEffectPrefab != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f); // Xoá hiệu ứng sau 0.5s
        }
        yield return new WaitForSeconds(1f); // Thời gian giữa các lần đánh
        isAttacking = false;
    }

    // Gọi hàm này khi enemy bị tấn công, ví dụ: TakeDamage(1);
}