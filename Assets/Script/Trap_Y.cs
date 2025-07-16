using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Y : MonoBehaviour
{
    public float start, end; // Điểm thấp nhất và cao nhất
    public float speed = 2f; // Tốc độ di chuyển
    private bool movingUp = true; // true: đi lên, false: đi xuống

    void Update()
    {
        // Swap nếu người dùng set start > end
        if (start > end)
        {
            float temp = start;
            start = end;
            end = temp;
        }

        // Lấy vị trí hiện tại
        float posY = transform.position.y;

        // Đảo hướng khi chạm giới hạn
        if (posY >= end)
        {
            movingUp = false;
        }
        else if (posY <= start)
        {
            movingUp = true;
        }

        // Tính toán hướng di chuyển
        Vector2 direction = movingUp ? Vector2.up : Vector2.down;

        // Di chuyển trap
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
