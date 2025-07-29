using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 20f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1")) // G?n Tag Player1 v�o Player
        {
            // G?i th�ng �i?p "Heal" �?n t?t c? component tr�n Player
            other.SendMessage("Heal", healAmount, SendMessageOptions.DontRequireReceiver);

            Debug.Log("?? Player nh?t m�u v� ��?c h?i " + healAmount);
            Destroy(gameObject); // Xo� c?c m�u sau khi nh?t
        }
    }
}
