using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLIM_X : MonoBehaviour
{
    public float start, end;
    private bool checkrigh;
    public Animator anie;

    [SerializeField] private GameObject slimePrefab; // Prefab slime để spawn
    [SerializeField] private GameObject goldPrefab;  // Prefab vàng để spawn
    [SerializeField] private int spawnCount = 3;     // Số lượng slime spawn ra

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hit"))
        {
            anie.SetTrigger("die");

            // Spawn slime con
            SpawnSlimes();

            // Spawn vàng
            SpawnGold();

            // Xóa slime cha sau 1s
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

    void Update()
    {
        var position_enermy = transform.position.x;
        if (position_enermy > end)
        {
            checkrigh = false;
        }
        if (position_enermy < start)
        {
            checkrigh = true;
        }
        if (checkrigh)
        {
            transform.Translate(Vector2.right * 2f * Time.deltaTime);
            transform.localScale = new Vector3(5, 5, 5);
        }
        else
        {
            transform.Translate(Vector2.left * 2f * Time.deltaTime);
            transform.localScale = new Vector3(-5, 5, 5);
        }
    }
}
