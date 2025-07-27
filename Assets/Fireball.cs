using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);

        // Bay theo hướng enemy đang quay mặt
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        rb.velocity = new Vector2(direction * speed, 0f);
    }
}
