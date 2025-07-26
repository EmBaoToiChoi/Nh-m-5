using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (player != null)
            transform.position = player.position;
    }
}
