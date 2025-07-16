using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float Move = 25f;
    [SerializeField] private float timeDestroy = 0.5f;
    

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }
    void MoveBullet()
    {
        transform.position += Move * Time.deltaTime * transform.right;

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu chạm Boss hoặc Enemy, hủy đạn
        if ( other.CompareTag("enermy"))
        {
            
            Destroy(gameObject);
        }
    }
}
