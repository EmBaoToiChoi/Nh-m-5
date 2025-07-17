using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRAP_X : MonoBehaviour
{
    public float start, end;  
    public float speed = 2f;  
    private bool movingRight = true; 

    

    void Update()
    {
        float posX = transform.position.x;

        
        if (posX > end) movingRight = false;
        if (posX < start) movingRight = true;

      
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
