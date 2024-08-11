using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public static bool isGameEnded;
    
    public const string MenuScene = "MainMenu";
    public const string LevelSelectorScene = "LevelSelector";
    
    public GameObject gameOverUi;
    public GameObject pauseMenuUi;
    public Text roundsText;
    public SceneChanger sceneChanger;
    public bool playBackgroundMusic = true;
    
    private float brains;

    private void Start()
    {
        isGameEnded = false;
        brains = 5f;
        AudioManager.Instance.stop("MenuBackground");
        if (playBackgroundMusic)
        {
            AudioManager.Instance.play("GamePlayBackground");
        }
    }

    private void Update()
    {
        if (isGameEnded)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggle();
        }
    }

    private void endGame()
    {
        gameOverUi.SetActive(true);
        roundsText.text = PlayerManager.Instance.rounds.ToString();
        isGameEnded = true;
        Debug.Log("Game over!");
        Time.timeScale = 0f;
    }

    public void toggle()
    {
        pauseMenuUi.SetActive(!pauseMenuUi.activeSelf);

        if (pauseMenuUi.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void retry()
    {
        Time.timeScale = 1f;
        sceneChanger.transitionTo(SceneManager.GetActiveScene().name);
    }

    public void menu()
    {
        Time.timeScale = 1f;
        sceneChanger.transitionTo(MenuScene);
    }

    public void wonLevel()
    {
        sceneChanger.transitionTo(LevelSelectorScene);
    }

    public void eatBrain()
    {
        brains -= 1;
        if (brains == 0)
        {
            endGame();
        }
    }
}