using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public float enemySpawnHeight = 30;
    public float spawnRange;
    public int enemyCount;
    public int waveNumber = 1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameOver)
        {
            enemyCount = FindObjectsOfType<Enemy>().Length;

            if (enemyCount == 0)
                SpawnEnemyWave(waveNumber);
        }
    }

    void SpawnEnemyWave(int nbrOfEnemiesToSpawn)
    {
        if (waveNumber != 1)
            Instantiate(powerupPrefab, GenerateSpawnPos(0), powerupPrefab.transform.rotation);

        for (int i = 0; i < nbrOfEnemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPos(enemySpawnHeight), enemyPrefab.transform.rotation);
        }

        waveNumber++;
    }

    private Vector3 GenerateSpawnPos(float height)
    {
        float spawnX = Random.Range(-spawnRange, spawnRange);
        float spawnZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(spawnX, height, spawnZ);
    }
}
