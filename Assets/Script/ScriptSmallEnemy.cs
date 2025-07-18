using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSmallEnemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 2f;
    public float speed = 2f;
    public int maxHealth = 1;
    public int attackDamage = 1;
    public float attackCooldown = 1f;
    private int currentHealth;
    private bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        // Gọi animation đánh nếu có, ví dụ: ani?.Play("SmallEnemyAttack");
        // Gây sát thương cho player (giả sử player có hàm TakeDamage)
        player.GetComponent<Player1>()?.TakeDamage(attackDamage);
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // Gọi animation bị thương nếu có, ví dụ: ani?.Play("SmallEnemyHurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Gọi animation chết nếu có, ví dụ: ani?.Play("SmallEnemyDie");
        Destroy(gameObject);
    }
}