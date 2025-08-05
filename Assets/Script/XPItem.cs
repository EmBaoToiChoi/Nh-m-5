using UnityEngine;

public class XPItem : MonoBehaviour
{
    public float xpAmount = 5f;

    [Header("Thông báo")]
    [SerializeField] private PickupMessageManager messageManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            XPManager.Instance.AddXP(xpAmount); // Gọi hệ thống cộng XP riêng của bạn

            Debug.Log("✅ Player nhận " + xpAmount + " kinh nghiệm");

            if (messageManager != null)
            {
                messageManager.ShowXPMessageStackable(xpAmount); // Cộng dồn XP
            }

            Destroy(gameObject);
        }
    }
}
