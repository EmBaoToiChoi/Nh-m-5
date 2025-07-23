using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 20f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1")) // G?n Tag Player1 vào Player
        {
            // G?i thông ði?p "Heal" ð?n t?t c? component trên Player
            other.SendMessage("Heal", healAmount, SendMessageOptions.DontRequireReceiver);

            Debug.Log("?? Player nh?t máu và ðý?c h?i " + healAmount);
            Destroy(gameObject); // Xoá c?c máu sau khi nh?t
        }
    }
}
