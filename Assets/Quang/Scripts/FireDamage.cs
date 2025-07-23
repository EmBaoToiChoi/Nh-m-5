using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public int damage = 10;

    void OnParticleCollision(GameObject other)
    {
        // Ki?m tra Tag trý?c
        if (other.CompareTag("Player1"))
        {
            // G?i hàm TakeFireDamage n?u có
            var player = other.GetComponent<MonoBehaviour>();
            if (player != null)
            {
                // Ki?m tra hàm t?n t?i r?i g?i b?ng SendMessage ð? không l?i
                other.SendMessage("TakeFireDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
