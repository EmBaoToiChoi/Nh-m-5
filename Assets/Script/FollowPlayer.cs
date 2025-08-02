using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform[] possiblePlayers; // 3 Transform của các player
    private Transform currentPlayer;

    void LateUpdate()
    {
        // Tìm player đang active
        foreach (Transform p in possiblePlayers)
        {
            if (p.gameObject.activeInHierarchy)
            {
                currentPlayer = p;
                break;
            }
        }

        // Follow
        if (currentPlayer != null)
        {
            Vector3 pos = currentPlayer.position;
            pos.z = transform.position.z; // giữ nguyên Z
            transform.position = pos;
        }
    }
}
