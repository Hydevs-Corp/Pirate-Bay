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
        WaveText.GetComponent<TMP_Text>().text = "Wave " + waveNumber + " is comming...";
        StartCoroutine(DisplayNewWaveText());
    }

    void SpawnEnemies()
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

        for (int i = 0; i < waveNumber * 1.51f; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            int enemyIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[enemyIndex];

            int lootIndex = Random.Range(0, loots.Count);
            GameObject lootPrefab = loots[lootIndex];

            enemiesAlive++;
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);


            enemy.GetComponent<EnemyController>().target = target;
            enemy.GetComponent<EnemyController>().loot = lootPrefab;

        }
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

    public void EnemyDied()
    {
        enemiesAlive--;
        print("Enemies alive: " + enemiesAlive);
        if (enemiesAlive <= 0)
        {
            SpawnWave();
        }
    }
}
