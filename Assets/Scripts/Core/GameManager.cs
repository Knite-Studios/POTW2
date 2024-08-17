using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        // Initialize game state and audio
        IsGameEnded = false;
        IsPaused = false;
        AudioManager.Instance.Stop("MenuBackground");
        if (playBackgroundMusic)
        {
            AudioManager.Instance.Play("GamePlayBackground");
        }
    }

    private void Update()
    {
        // Handle pause menu toggle
        if (IsGameEnded) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void EndGame()
    {
        // Set up game over state
        if (IsGameEnded) return;

        gameOverUi.SetActive(true);
        roundsText.text = PlayerManager.Instance.rounds.ToString();
        IsGameEnded = true;
        Debug.Log("Game over!");
        Time.timeScale = 0f;
        PauseAudio();
    }

    public void TogglePauseMenu()
    {
        IsPaused = !IsPaused;
        
        // Toggle pause menu and time scale
        pauseMenuUi.SetActive(IsPaused);
        Time.timeScale = IsPaused ? 0f : 1f;

        // Pause or unpause audio
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
        // Restart the current level
        Time.timeScale = 1f;
        IsPaused = false;
        UnpauseAudio();
        sceneChanger.TransitionTo(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        // Go back to the main menu
        Time.timeScale = 1f;
        IsPaused = false;
        UnpauseAudio();
        sceneChanger.TransitionTo(MenuScene);
    }

    public void WonLevel()
    {
        // Transition to level selector after winning
        sceneChanger.TransitionTo(LevelSelectorScene);
    }
}