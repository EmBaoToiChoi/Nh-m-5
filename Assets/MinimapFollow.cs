using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform target; // Gán Player vào đây trong Inspector

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPos = target.position;
        newPos.z = transform.position.z; // giữ nguyên độ cao camera
        transform.position = newPos;
    }
}
