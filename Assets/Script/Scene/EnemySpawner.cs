using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform spawnPosition;
        [HideInInspector] public GameObject currentEnemy;
    }

    public GameObject enemyPrefab; // มอนสเตอร์ที่จะ Spawn
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>(); // จุดเกิดทั้งหมด

    public float spawnInterval = 5f; // เวลาระหว่างการเช็ค (วินาที)

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            foreach (SpawnPoint point in spawnPoints)
            {
                if (point.currentEnemy == null) // ถ้าไม่มีมอน หรือมอนตายไปแล้ว
                {
                    GameObject newEnemy = Instantiate(enemyPrefab, point.spawnPosition.position, Quaternion.identity);
                    point.currentEnemy = newEnemy;
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
