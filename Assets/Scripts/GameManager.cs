using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ---------------------- General variables ---------------------
    public bool playing = true;
    private bool paused = false;
    private GameObject player;
    private Vector3 playerStartPos = new Vector3(0, 0.72f, 0);
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }
 
            return _instance;
        }
    }

    // --------- Powerup varibles and their corresponding UI Displays ------------
    [SerializeField] int toolAmount = 0;
    [SerializeField] GameObject toolCount;
    private int addToolAmount = 1;

    [SerializeField] float energyAmount = 0.5f;
    [SerializeField] GameObject energyAmountSlider;
    private float addEnergyAmount = 0.1f;
    
    [SerializeField] GameObject energyBeam;
    private Vector3 energyBeamOffset = new Vector3(0, 0.5f, 0);

    // -------------------- UI Screens -------------------------------
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject settingsInfo;


    // -------------------- Spawning variabls -------------------------
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] powerupPrefabs;

    private float XspawnRange = 25.0f;
    private float ZspawnRange = 25.0f;

    private float enemySpawnMinTime = 4.0f;
    private float enemySpawnMaxTime = 10.0f;
    private float enemyYSpawnPos = 0.75f;
    private int maxEnemyCount = 6;
    private float enemySpeedMultiplier = 1;
    private float addEnemySpeedAmount = 0.1f;

    private float powerupSpawnMinTime = 2.0f;
    private float powerupSpawnMaxTime = 5.0f;
    private float powerupYSpawnPos = 0.7f;
    private int maxPowerupCount = 8;

    // -------------- Audio Clips ---------------------------
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip powerupAudioClip;
    [SerializeField] AudioClip buttonAudioClip;
    [SerializeField] AudioClip enemyDeathAudioClip;
    [SerializeField] AudioClip playerDeathAudioClip;
    [SerializeField] AudioClip shootAudioClip;


    void Awake()
    {
        _instance = this;
        LoadStartScreen();
    }

    public void LoadStartScreen() {
        playing = false;
        startScreen.gameObject.SetActive(true);
        settingsInfo.gameObject.SetActive(true);
        player = GameObject.Find("Player");
        player.transform.position = playerStartPos;
        player.gameObject.SetActive(true);

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            Destroy(enemy);
        }

        gameOverScreen.gameObject.SetActive(false);
        pauseScreen.gameObject.SetActive(false);
        energyAmountSlider.gameObject.SetActive(false);
        toolCount.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    public void StartGame()
    {
        audioSource.PlayOneShot(buttonAudioClip);
        toolAmount = 0;
        energyAmount = 0.5f;
        enemySpeedMultiplier = 0;
        AddEnergy(0);
        AddTool(0);
        playing = true;
        
        gameOverScreen.gameObject.SetActive(false);
        pauseScreen.gameObject.SetActive(false);
        startScreen.gameObject.SetActive(false);
        settingsInfo.gameObject.SetActive(false);
        
        energyAmountSlider.gameObject.SetActive(true);
        toolCount.gameObject.SetActive(true);

        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }

    public void GameOver() {
        audioSource.PlayOneShot(playerDeathAudioClip);
        musicSource.Pause();
        playing = false;
        gameOverScreen.gameObject.SetActive(true);
        foreach(GameObject powerup in GameObject.FindGameObjectsWithTag("Powerup")) {
            Destroy(powerup);
        }
    }

    public void Reset() {
        audioSource.PlayOneShot(buttonAudioClip);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TogglePause() {
        if (!playing && paused || playing && !paused) {
            paused = !paused;
            playing = !paused;
            Time.timeScale = paused ? 0 : 1; 
            pauseScreen.gameObject.SetActive(paused);
            settingsInfo.gameObject.SetActive(paused);
        }
    }

    public void Powerup(PowerupType type) {
        audioSource.PlayOneShot(powerupAudioClip);
        if (type == PowerupType.Energy) {
            AddEnergy(addEnergyAmount);
        } else if (type == PowerupType.Tool) {
            AddTool(addToolAmount);
        }
    }

    public void AddEnergy(float amount) {
        if (energyAmount <= 1) {
            energyAmount += amount;
            energyAmountSlider.gameObject.GetComponent<Slider>().value = energyAmount;
        }
    }

    public void ShootRay() {
        audioSource.PlayOneShot(shootAudioClip);
        if (energyAmount >= 0) {
            energyAmount -= addEnergyAmount;
            energyAmountSlider.gameObject.GetComponent<Slider>().value = energyAmount;

            Instantiate(energyBeam, player.transform.position + energyBeamOffset, player.transform.rotation * energyBeam.transform.rotation);
        }
    }

    private void AddTool(int amount) {
        toolAmount += amount;
        toolCount.GetComponentInChildren<TextMeshProUGUI>().text = "x" + toolAmount;
    }

    IEnumerator SpawnEnemy() {
        while (playing) {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemyCount) {                
                Vector3 spawnPos = new Vector3(Random.Range(-XspawnRange,XspawnRange), enemyYSpawnPos, Random.Range(-ZspawnRange,ZspawnRange));
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPos, enemyPrefabs[randomIndex].transform.rotation);
                enemy.gameObject.GetComponent<Enemy>().speed *= enemySpeedMultiplier;

                float randomTime = Random.Range(enemySpawnMinTime, enemySpawnMaxTime);
                enemySpeedMultiplier += addEnemySpeedAmount;
                yield return new WaitForSeconds(randomTime);
            } else {
                float randomTime = Random.Range(enemySpawnMinTime, enemySpawnMaxTime);
                yield return new WaitForSeconds(randomTime);
            }
        }
    }

    IEnumerator SpawnPowerup() {
        while (playing) {
            if (GameObject.FindGameObjectsWithTag("Powerup").Length < maxPowerupCount) {
                Vector3 spawnPos = new Vector3(Random.Range(-XspawnRange,XspawnRange), powerupYSpawnPos, Random.Range(-ZspawnRange,ZspawnRange));
                int randomIndex = Random.Range(0, powerupPrefabs.Length);
                Instantiate(powerupPrefabs[randomIndex], spawnPos, powerupPrefabs[randomIndex].transform.rotation);

                float randomTime = Random.Range(powerupSpawnMinTime, powerupSpawnMaxTime);
                yield return new WaitForSeconds(randomTime);
            } else {
                float randomTime = Random.Range(enemySpawnMinTime, enemySpawnMaxTime);
                yield return new WaitForSeconds(randomTime);
            }
        }
    }

}
