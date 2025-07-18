using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointEnemy : MonoBehaviour
{
    public GameObject enemyPrefab; // Gán prefab enemy trong Inspector
    public float spawnInterval = 5f;
    public int maxSpawnCount = 10;

    private int spawnCount = 0;
    private float timer = 0f;

    void Update()
    {
        if (spawnCount >= maxSpawnCount) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        spawnCount++;
    }
}