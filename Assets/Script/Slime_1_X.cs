using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_1_X : MonoBehaviour
{
    public float start, end;
    private bool checkrigh;
    public Animator anie;
void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hit"))
        {
            anie.SetTrigger("die");
            Destroy(this.gameObject, 1f);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         var position_enermy = transform.position.x;
        if(position_enermy > end){
            checkrigh = false;
        }
        if(position_enermy < start){
            checkrigh = true;

        }
        if(checkrigh){
            transform.Translate(Vector2.right * 2f * Time.deltaTime);
            transform.localScale = new Vector3(5,5,5);
        } else {transform.Translate(Vector2.left * 2f * Time.deltaTime);
        transform.localScale = new Vector3(-5,5,5);}
    }
}
