using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public static bool IsGameEnded { get; private set; }
    public static bool IsPaused { get; private set; }

    public const string MenuScene = "MainMenu";
    public const string LevelSelectorScene = "LevelSelector";

    [SerializeField] private GameObject gameOverUi;
    [SerializeField] private GameObject pauseMenuUi;
    [SerializeField] private Text roundsText;
    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private bool playBackgroundMusic = true;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        IsGameEnded = false;
        IsPaused = false;
        Time.timeScale = 1f;
        AudioManager.Instance.Stop("MenuBackground");
        if (playBackgroundMusic)
        {
            AudioManager.Instance.Play("GamePlayBackground");
        }
    }

    private void Update()
    {
        if (IsGameEnded) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void EndGame()
    {
        if (IsGameEnded) return;

        gameOverUi.SetActive(true);
        if (roundsText != null && PlayerManager.Instance != null)
        {
            roundsText.text = PlayerManager.Instance.rounds.ToString();
        }
        IsGameEnded = true;
        Debug.Log("Game over!");
        Time.timeScale = 0f;
        PauseAudio();
    }

    public void TogglePauseMenu()
    {
        IsPaused = !IsPaused;
        if (pauseMenuUi != null)
        {
            pauseMenuUi.SetActive(IsPaused);
        }
        Time.timeScale = IsPaused ? 0f : 1f;

        if (IsPaused)
        {
            PauseAudio();
        }
        else
        {
            UnpauseAudio();
        }
    }

    private void PauseAudio()
    {
        AudioManager.Instance.PauseAll();
    }

    private void UnpauseAudio()
    {
        AudioManager.Instance.UnpauseAll();
    }

    public void Retry()
    {
        StartCoroutine(RetryCoroutine());
    }

    private IEnumerator RetryCoroutine()
    {
        // Reset game state
        IsGameEnded = false;
        IsPaused = false;
        Time.timeScale = 1f;

        // Unpause audio
        UnpauseAudio();

        // Clean up existing objects
        yield return StartCoroutine(CleanupScene());

        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        // Wait for the scene to load
        yield return null;

        // Re-initialize the game
        InitializeGame();

        // Reset other managers
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.ResetRounds();
        if (HypeManager.Instance != null)
            HypeManager.Instance.ResetHype();
        if (WaveSpawner.Instance != null)
            WaveSpawner.Instance.ResetWaves();
    }

    private IEnumerator CleanupScene()
    {
        // Destroy all non-persistent objects
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (!obj.scene.isLoaded)
                continue;

            if (obj.hideFlags == HideFlags.DontSave || obj.hideFlags == HideFlags.HideAndDontSave)
                continue;

            if (obj.transform.parent == null)
                Destroy(obj);
        }

        // Wait for a frame to ensure objects are destroyed
        yield return null;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        UnpauseAudio();
        if (sceneChanger != null)
        {
            sceneChanger.TransitionTo(MenuScene);
        }
        else
        {
            SceneManager.LoadScene(MenuScene);
        }
    }

    public void WonLevel()
    {
        if (sceneChanger != null)
        {
            sceneChanger.TransitionTo(LevelSelectorScene);
        }
        else
        {
            SceneManager.LoadScene(LevelSelectorScene);
        }
    }
}