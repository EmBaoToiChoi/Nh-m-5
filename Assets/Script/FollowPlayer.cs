using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform[] possiblePlayers; // 3 Transform của các player
    private Transform currentPlayer;

    void LateUpdate()
    {
        // Nếu currentPlayer null hoặc không còn active thì tìm lại
        if (currentPlayer == null || !currentPlayer.gameObject.activeInHierarchy)
        {
            currentPlayer = null; // reset

            foreach (Transform p in possiblePlayers)
            {
                if (p != null && p.gameObject.activeInHierarchy)
                {
                    currentPlayer = p;
                    break;
                }
            }
        }

        // Follow nếu tìm được player hợp lệ
        if (currentPlayer != null)
        {
            Vector3 pos = currentPlayer.position;
            pos.z = transform.position.z; // giữ nguyên Z
            transform.position = pos;
        }
    }
}
