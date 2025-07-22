using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBow : MonoBehaviour
{
    
    [SerializeField] private float Move = 25f;
    [SerializeField] private float timeDestroy = 0.5f;
    

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    
    void Update()
    {
        MoveBullet();
    }
    void MoveBullet()
    {
        transform.position += transform.right * Move * Time.deltaTime;

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if ( other.CompareTag("enermy"))
        {
            Destroy(gameObject);
        }
    }
}
