using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public static int zombiesAlive;

    [SerializeField] private Text dangerText;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawnPositions;

    private float countDown;
    private int waveIndex;

    private void Start()
    {
        // Initialize wave spawner
        countDown = timeBetweenWaves;
        zombiesAlive = 0;
        if (dangerText != null)
        {
            dangerText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for wave completion and start new waves
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
            StartCoroutine(SpawnWave());
            countDown = timeBetweenWaves;
        }
    }

    private IEnumerator SpawnWave()
    {
        // Spawn a wave of zombies
        PlayerManager.Instance.rounds++;
        Wave currentWave = waves[waveIndex];

        yield return StartCoroutine(ShowDangerText());

        float waveDuration = currentWave.numberZombies * currentWave.spawnDelay;
        WaveBar.Instance.startBar(waveDuration);

        for (int i = 0; i < currentWave.numberZombies; i++)
        {
            SpawnEnemy(currentWave.zombie);
            yield return new WaitForSeconds(currentWave.spawnDelay);
        }

        waveIndex++;
    }

    private IEnumerator ShowDangerText()
    {
        // Display and fade out the danger text
        if (dangerText != null)
        {
            dangerText.gameObject.SetActive(true);
            dangerText.text = "Danger Incoming!";
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
        // Spawn a single enemy at a random position
        int randomSpawnIndex = Random.Range(0, spawnPositions.Length);
        Instantiate(prefab, spawnPositions[randomSpawnIndex].position, Quaternion.identity);
        zombiesAlive++;
    }
}