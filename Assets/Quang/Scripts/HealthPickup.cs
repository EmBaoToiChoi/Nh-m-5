using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 5f;

    [Header("Thông báo")]
    [SerializeField] private PickupMessageManager messageManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            other.SendMessage("Heal", healAmount, SendMessageOptions.DontRequireReceiver);

            Debug.Log("✅ Player nhặt máu và hồi " + healAmount);

            if (messageManager != null)
            {
                messageManager.ShowHealthMessageStackable(healAmount); // Cộng dồn máu
            }

            Destroy(gameObject);
        }
    }
}
