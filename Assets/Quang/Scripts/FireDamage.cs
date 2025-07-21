using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public int damage = 10;

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player1"))
        {
            Player1 player = other.GetComponent<Player1>();
            if (player != null)
            {
                player.TakeFireDamage(damage); // G?i hàm b?n s?p thêm
            }
        }
    }
}
