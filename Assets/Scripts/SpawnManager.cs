using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs;

    private float XspawnRange = 25.0f;
    private float ZspawnRange = 25.0f;

    private float enemySpawnMinTime = 4.0f;
    private float enemySpawnMaxTime = 10.0f;
    private float enemyYSpawnPos = 0.25f;
    private int maxEnemyCount = 6;

    private float powerupSpawnMinTime = 4.0f;
    private float powerupSpawnMaxTime = 10.0f;
    private float powerupYSpawnPos = 0.7f;
    private int maxPowerupCount = 8;


    // Start is called before the first frame update
    void Start() {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }

    // Update is called once per frame
    void Update() {
    
    }

    IEnumerator SpawnEnemy() {
        while (true) {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemyCount) {
                float randomTime = Random.Range(enemySpawnMinTime, enemySpawnMaxTime);
                yield return new WaitForSeconds(randomTime);
                
                Vector3 spawnPos = new Vector3(Random.Range(-XspawnRange,XspawnRange), enemyYSpawnPos, Random.Range(-ZspawnRange,ZspawnRange));
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs[randomIndex], spawnPos, enemyPrefabs[randomIndex].transform.rotation);
            } else {
                float randomTime = Random.Range(enemySpawnMinTime, enemySpawnMaxTime);
                yield return new WaitForSeconds(randomTime);
            }
        }
    }

    IEnumerator SpawnPowerup() {
        while (true) {
            if (GameObject.FindGameObjectsWithTag("Powerup").Length < maxPowerupCount) {
                float randomTime = Random.Range(powerupSpawnMinTime, powerupSpawnMaxTime);
                yield return new WaitForSeconds(randomTime);

                Vector3 spawnPos = new Vector3(Random.Range(-XspawnRange,XspawnRange), powerupYSpawnPos, Random.Range(-ZspawnRange,ZspawnRange));
                int randomIndex = Random.Range(0, powerupPrefabs.Length);
                Instantiate(powerupPrefabs[randomIndex], spawnPos, powerupPrefabs[randomIndex].transform.rotation);
            } else {
                float randomTime = Random.Range(enemySpawnMinTime, enemySpawnMaxTime);
                yield return new WaitForSeconds(randomTime);
            }
        }
    }
}
