using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public static bool isGameEnded;
    public string menuScene = "MainMenu";
    public string levelSelectorScene = "LevelSelector";
    public GameObject gameOverUi;
    public GameObject pauseMenuUi;
    public Text roundsText;
    public SceneChanger sceneChanger;
    public bool playBackgroundMusic = true;
    private float brains;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            Debug.LogError("More than one game manager in the scene!");
        }
        else
        {
            instance = this;
        }

        isGameEnded = false;
        brains = 5f;
        AudioManager.instance.stop("MenuBackground");
        if (playBackgroundMusic)
        {
            AudioManager.instance.play("GamePlayBackground");
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
        roundsText.text = PlayerManager.instance.rounds.ToString();
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
        sceneChanger.transitionTo(menuScene);
    }

    public void wonLevel()
    {
        sceneChanger.transitionTo(levelSelectorScene);
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