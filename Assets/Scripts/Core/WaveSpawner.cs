using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : Singleton<WaveSpawner>
{
    public static int zombiesAlive;

    [SerializeField] private Text dangerText;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawnPositions;

    private float countDown;
    private int waveIndex;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        InitializeWaveSpawner();
    }

    private void InitializeWaveSpawner()
    {
        countDown = timeBetweenWaves;
        zombiesAlive = 0;
        waveIndex = 0;
        if (dangerText != null)
        {
            dangerText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (zombiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            GameManager.Instance.WonLevel();
            enabled = false;
            return;
        }

        countDown -= Time.deltaTime;

        if (countDown <= 0f)
        {
            spawnCoroutine = StartCoroutine(SpawnWave());
            countDown = timeBetweenWaves;
        }
    }

    private IEnumerator SpawnWave()
    {
        PlayerManager.Instance.rounds++;
        Wave currentWave = waves[waveIndex];

        yield return StartCoroutine(ShowDangerText());

        float waveDuration = currentWave.numberZombies * currentWave.spawnDelay;
        if (WaveBar.Instance != null)
        {
            WaveBar.Instance.StartBar(waveDuration);
        }

        for (int i = 0; i < currentWave.numberZombies; i++)
        {
            SpawnEnemy(currentWave.zombie);
            yield return new WaitForSeconds(currentWave.spawnDelay);
        }

        waveIndex++;
    }

    private IEnumerator ShowDangerText()
    {
        if (dangerText != null)
        {
            dangerText.gameObject.SetActive(true);
            dangerText.text = "WAVE INCOMING!";
            dangerText.color = new Color(dangerText.color.r, dangerText.color.g, dangerText.color.b, 1f);

            yield return new WaitForSeconds(2f);

            float fadeDuration = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                dangerText.color = new Color(dangerText.color.r, dangerText.color.g, dangerText.color.b, alpha);
                yield return null;
            }

            dangerText.gameObject.SetActive(false);
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        if (spawnPositions == null || spawnPositions.Length == 0)
        {
            Debug.LogError("No spawn positions set for WaveSpawner");
            return;
        }

        int randomSpawnIndex = Random.Range(0, spawnPositions.Length);
        if (spawnPositions[randomSpawnIndex] != null)
        {
            Instantiate(prefab, spawnPositions[randomSpawnIndex].position, Quaternion.identity);
            zombiesAlive++;
        }
        else
        {
            Debug.LogError($"Spawn position at index {randomSpawnIndex} is null");
        }
    }

    public void ResetWaves()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        InitializeWaveSpawner();

        // Destroy all existing zombies
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (GameObject zombie in zombies)
        {
            Destroy(zombie);
        }

        zombiesAlive = 0;
    }
}