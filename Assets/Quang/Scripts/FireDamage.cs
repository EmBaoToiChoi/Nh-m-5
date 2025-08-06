using UnityEngine;

public class FireBreathDamage : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            Player1 player = collision.GetComponent<Player1>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
