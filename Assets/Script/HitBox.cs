using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 1; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enermy"))
        {
            Enemy1Script enemy = other.GetComponent<Enemy1Script>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        WormScript worm = other.GetComponent<WormScript>();
        if (worm != null)
        {
            worm.TakeDamage(damage);
        }
    }
}
