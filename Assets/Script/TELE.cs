using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TELE : MonoBehaviour
{
    [Header("Enemy Prefab & Spawn Points")]
    [SerializeField] private GameObject enemyPrefab;     // Prefab của quái
    [SerializeField] private Transform[] spawnPoints;    // Các vị trí spawn

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 5f;   // Thời gian giữa mỗi lần spawn
    [SerializeField] private float enemyLifetime = 10f;  // Thời gian tồn tại của quái

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("TELE: Chưa gán enemyPrefab!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("TELE: Chưa gán spawnPoints!");
            return;
        }

        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        // Chọn ngẫu nhiên 1 vị trí spawn
        int pointIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[pointIndex];

        if (selectedPoint == null)
        {
            Debug.LogWarning("TELE: SpawnPoint bị null.");
            return;
        }

        // Spawn quái
        GameObject spawnedEnemy = Instantiate(enemyPrefab, selectedPoint.position, Quaternion.identity);

        // Xoá quái sau thời gian tồn tại
        Destroy(spawnedEnemy, enemyLifetime);
    }
}
