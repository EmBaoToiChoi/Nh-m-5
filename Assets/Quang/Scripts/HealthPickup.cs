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
            if (ThanhMauPl_1.Instance != null)
            {
                ThanhMauPl_1.Instance.Heal(healAmount);
                Debug.Log("✅ Player nhặt máu và hồi " + healAmount);

                if (messageManager != null)
                {
                    messageManager.ShowHealthMessageStackable(healAmount); // Cộng dồn máu
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy ThanhMauPl_1.Instance");
            }

            Destroy(gameObject);
        }
    }


}
