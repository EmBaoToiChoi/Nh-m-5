using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public int damage = 10;
    public float damageCooldown = 3f;
    private float lastDamageTime;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1")) // tag ðúng c?a b?n
        {
            if (Time.time - lastDamageTime > damageCooldown)
            {
                other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                lastDamageTime = Time.time;
                Debug.Log("?? Enemy ðánh trúng player!");
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            if (Time.time - lastDamageTime > damageCooldown)
            {
                other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                lastDamageTime = Time.time;
                Debug.Log("?? Enemy liên t?c gây damage!");
            }
        }
    }
}
