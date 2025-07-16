using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_X : MonoBehaviour
{
    public float start, end;  
    public float speed = 2f;  
    private bool checkrigh = true;

    void Update()
    {
        // Di chuyển
        Vector2 direction = checkrigh ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);

        // Lấy vị trí hiện tại
        float posX = transform.position.x;

        // Đổi hướng khi chạm biên
        if (checkrigh && posX >= end)
        {
            checkrigh = false;
            transform.position = new Vector2(end, transform.position.y); // đảm bảo không vượt biên
        }
        else if (!checkrigh && posX <= start)
        {
            checkrigh = true;
            transform.position = new Vector2(start, transform.position.y); // đảm bảo không vượt biên
        }
    }
}
