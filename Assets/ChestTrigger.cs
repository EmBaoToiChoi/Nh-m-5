using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    [Header("Player cần chạm vào")]
    public string playerTag = "Player1";

    [Header("Các mũi tên sẽ được hiện ra")]
    public GameObject[] arrows;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag(playerTag))
        {
            triggered = true;

            foreach (GameObject arrow in arrows)
            {
                if (arrow != null)
                    arrow.SetActive(true); // Hiện mũi tên
            }

            // Nếu muốn chest biến mất thì bỏ comment dòng này
            // gameObject.SetActive(false);
        }
    }
}
