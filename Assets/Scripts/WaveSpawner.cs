using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public static int zombiesAlive;
    public Transform enemyPrefab;
    public Text countDownText;
    public float timeBetweenWaves = 5f;
    public Wave[] waves;
    public Transform[] spawnPositions;
    private float countDown;
    private int waveIndex;

    private void Start()
    {
        countDown = timeBetweenWaves;
        zombiesAlive = 0;
    }

    private void Update()
    {
        if (zombiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            // TODO: do animation here
            GameManager.Instance.wonLevel();
            enabled = false;
        }

        if (countDown <= 0f)
        {
            StartCoroutine(spawnWave());
            countDown = timeBetweenWaves;
            return;
        }


        countDown -= Time.deltaTime;
        countDownText.text = Mathf.Round(countDown).ToString();
    }

    private IEnumerator spawnWave()
    {
        PlayerManager.Instance.rounds++;
        var wave = waves[waveIndex];
        var t = 0f;
        t += wave.numberZombies * wave.spawnDelay;
        WaveBar.Instance.startBar(t);

        for (var i = 0; i < wave.numberZombies; i++)
        {
            spawnEnemy(wave.zombie);
            yield return new WaitForSeconds(wave.spawnDelay);
        }

        waveIndex++;
    }

    private void spawnEnemy(GameObject prefab)
    {
        var x = Random.Range(0, 5);
        Instantiate(prefab, spawnPositions[x].position, Quaternion.Euler(0, 0, 0));
        zombiesAlive++;
    }
}