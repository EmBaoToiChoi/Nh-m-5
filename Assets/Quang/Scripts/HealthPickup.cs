using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 20; // S? máu c?ng cho Player

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Va ch?m v?i: " + other.name);

        if (other.CompareTag("Player1"))
        {
            Debug.Log("Player nh?t máu!");
            Player1 player = other.GetComponent<Player1>();
            if (player != null)
            {
                player.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }

}
