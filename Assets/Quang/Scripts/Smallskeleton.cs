using UnityEngine;

public class SmallSkeleton : MonoBehaviour
{
    public int health = 50;
    public GameObject smallSkeletonPrefab;
    public bool canRespawnSmall = false; // Ch? sinh 1 l?n

    private bool isDead = false;

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (canRespawnSmall && smallSkeletonPrefab != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 pos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                GameObject skel = Instantiate(smallSkeletonPrefab, pos, Quaternion.identity);

                // Skeleton c?p 2 s? không respawn n?a
                SmallSkeleton script = skel.GetComponent<SmallSkeleton>();
                if (script != null) script.canRespawnSmall = false;
            }
        }

        Destroy(gameObject);
    }
}
