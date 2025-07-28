using UnityEngine;

public class ManaPickup : MonoBehaviour
{
    public float ManaAmount = 20f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1")) // Gan Tag Player1 vào Player
        {
            // Gui thông ðiep "Manal" ð?n t?t c? component trên Player
            other.SendMessage("Heal", ManaAmount, SendMessageOptions.DontRequireReceiver);

            Debug.Log("?? Player nh?t máu và ðý?c h?i " + ManaAmount);
            Destroy(gameObject); // Xoá cuc mana sau khi nhat
        }
    }
}
