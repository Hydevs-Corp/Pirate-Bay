using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public bool waveActive = false;
    public List<GameObject> enemyPrefabs;
    public float spawnInterval = 50f;

    public List<GameObject> loots;

    public List<Transform> spawnPoints;
    public GameObject target;

    private int enemiesAlive = 0;

    private int waveNumber = 0;
    private GameObject NewWave;
    private TMP_Text RemainingEnemiesText;
    private GameObject WaveText;

    void Start()
    {

        WaveText = GameObject.Find("WaveText");
        NewWave = GameObject.Find("NewWave");
        NewWave.GetComponent<CanvasGroup>().alpha = 0;
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }
        if (waveActive)
            SpawnWave();

    }


    void SpawnWave()
    {

        waveNumber++;
        WaveText.GetComponent<TMP_Text>().text = "Wave " + waveNumber + " is coming...";
        StartCoroutine(DisplayNewWaveText());
    }

    public void SpawnEnemies()
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

        int enemyToSpawn = Mathf.Max(2, waveNumber * 2);

        var sortedSpawnPoints = new List<Transform>(spawnPoints);
        sortedSpawnPoints.Sort((a, b) => -Vector3.Distance(a.position, target.transform.position).CompareTo(Vector3.Distance(b.position, target.transform.position)));
        for (int i = 0; i < sortedSpawnPoints.Count; i++)
        {
            if (Vector3.Distance(sortedSpawnPoints[i].position, target.transform.position) < 40)
            {
                sortedSpawnPoints.RemoveAt(i);
                i--;
            }
        }

        print("Sorted spawn points length: " + sortedSpawnPoints.Count);

        if (sortedSpawnPoints.Count == 0)
        {
            sortedSpawnPoints = new List<Transform>(spawnPoints);
        }

        print("Spawning " + enemyToSpawn + " enemies");

        for (int i = 0; i < enemyToSpawn; i++)
        {
            Transform spawnPoint = sortedSpawnPoints[i % spawnPoints.Count];
            print("Spawning enemy at " + spawnPoint.position);


            int enemyIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[enemyIndex];

            int lootIndex = Random.Range(0, loots.Count);
            GameObject lootPrefab = loots[lootIndex];

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);


            enemy.GetComponent<EnemyController>().target = target;
            enemy.GetComponent<EnemyController>().loot = lootPrefab;

            enemiesAlive++;
        }
        DisplayRemainingEmeniesText();
        return;
    }

    IEnumerator DisplayNewWaveText()
    {
        CanvasGroup canvasGroup = NewWave.GetComponent<CanvasGroup>();
        float duration = 1f;

        // Fade in
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(2);

        // Fade out
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
        canvasGroup.alpha = 0;
        SpawnEnemies();
    }
    private void DisplayRemainingEmeniesText()
    {
        RemainingEnemiesText = GameObject.Find("RemainingEnemiesText").GetComponent<TMP_Text>();
        RemainingEnemiesText.text = "" + enemiesAlive;
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        DisplayRemainingEmeniesText();
        if (enemiesAlive <= 0)
        {
            SpawnWave();
        }
    }
}
