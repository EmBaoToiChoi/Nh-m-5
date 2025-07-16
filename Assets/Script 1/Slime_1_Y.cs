using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_1_Y : MonoBehaviour
{
    [SerializeField] private GameObject goldPrefab;
    [SerializeField] private int spawnCount = 3;
    [SerializeField] private GameObject slimePrefab;
    public float start, end;
 private bool checkdoc;
 public Animator anie;
 void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hit"))
        {
            anie.SetTrigger("die");
            SpawnSlimes();
            SpawnGold();
            Destroy(this.gameObject, 1f);
        }
    }
    void SpawnSlimes()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = transform.position + (Vector3)(Random.insideUnitCircle * 0.5f);
            GameObject slimeCon = Instantiate(slimePrefab, spawnPos, Quaternion.identity);

            // Giảm kích thước slime con
            slimeCon.transform.localScale = transform.localScale * 0.5f;
        }
    }
    void SpawnGold()
    {
        // Tọa độ spawn vàng (ở chính giữa slime cha)
        Vector3 spawnPos = transform.position;
        Instantiate(goldPrefab, spawnPos, Quaternion.identity);
    }


    // Update is called once per frame
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
