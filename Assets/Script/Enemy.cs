using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform tf;

    [Header("Tuần tra")]
    [SerializeField] private float start = -3f;
    [SerializeField] private float end = 3f;
    [SerializeField] private float speed = 2f;
    private bool isRight = true;

    [Header("Phân tách")]
    [SerializeField] private GameObject smallEnemyPrefab;
    [SerializeField] private int numberOfSmallEnemies = 3;
    [SerializeField] private float spawnRadius = 0.5f;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (tf == null) tf = transform;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (tf.position.x <= start)
        {
            isRight = true;
            Flip(true);
        }
        else if (tf.position.x >= end)
        {
            isRight = false;
            Flip(false);
        }

        tf.Translate((isRight ? Vector3.right : Vector3.left) * speed * Time.deltaTime);
    }

    void Flip(bool facingRight)
    {
        Vector3 scale = tf.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        tf.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Enemy đã va chạm với Player!");

            PlayerController Player = other.GetComponent<PlayerController>();
            if (Player != null)
            {
                Player.heal(20); // Hoặc giam máu tùy game bạn
            }
        }
    }

    public void TakeDamage(int amount)
    {
        Die();
    }

    public void Die()
    {
        Debug.Log("💀 Enemy đã chết và sẽ phân tách thành enemy nhỏ.");

        if (smallEnemyPrefab != null)
        {
            for (int i = 0; i < numberOfSmallEnemies; i++)
            {
                Vector2 spawnPosition = (Vector2)tf.position + Random.insideUnitCircle * spawnRadius;
                Instantiate(smallEnemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Chưa gán prefab của enemy nhỏ!");
        }

        Destroy(gameObject);
    }
}
