using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> enemyPrefabs;
    public float spawnInterval = 50f;
    public List<Transform> spawnPoints;
    public GameObject target;

    private int enemiesAlive = 0;

    void Start()
    {
        SpawnWave();
    }


    void SpawnWave()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned.");
            return;
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Transform spawnPoint = spawnPoints[i];

            int enemyIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[enemyIndex];

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemiesAlive++;
            enemy.GetComponent<EnemyController>().target = target;

        }
        return;
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            SpawnWave();
        }
    }
}
