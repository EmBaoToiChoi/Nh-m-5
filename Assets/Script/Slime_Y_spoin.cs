using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Y_spoin : MonoBehaviour
{
    [SerializeField] private GameObject goldPrefab;
    public float start, end;
    private bool checkdoc;
    public Animator anie;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hit"))
        {
            anie.SetTrigger("die");
            SpawnGold();
            Destroy(this.gameObject, 1f);
        }
    }
    void SpawnGold()
    {
        // Tọa độ spawn vàng (ở chính giữa slime cha)
        Vector3 spawnPos = transform.position;
        Instantiate(goldPrefab, spawnPos, Quaternion.identity);
    }
    void Update()
    {
        var position_enermy = transform.position.y;
        if(position_enermy > end){
            checkdoc = false;
        }
        if(position_enermy < start ){
            checkdoc = true;
        }

        if(checkdoc == true){
            transform.Translate(Vector2.up * 2f * Time.deltaTime);
            anie.SetBool("chayw", true);
      
        }else{anie.SetBool("chayw", false);}
        if(checkdoc == false){
            transform.Translate(Vector2.down * 2f * Time.deltaTime);
            anie.SetBool("chays", true);

       } else{anie.SetBool("chays", false);}
    }
}
