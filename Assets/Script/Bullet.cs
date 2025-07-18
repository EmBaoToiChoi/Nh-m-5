using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float Move = 25f;
    [SerializeField] private float timeDestroy = 0.5f;
    [SerializeField] private int damage = 15; // Thêm sát thương

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.position += transform.right * Move * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu chạm Enemy thường
        if (other.CompareTag("enermy"))
        {
            HealthSystem enemyHealth = other.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        // Nếu chạm Boss
        if (other.CompareTag("Boss"))
        {
            HealthSystem bossHealth = other.GetComponent<HealthSystem>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
