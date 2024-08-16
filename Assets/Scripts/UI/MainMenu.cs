using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public string sceneToLoad = "GamePlay";

    private void Start()
    {
        if (AudioManager.Instance.IsPlaying("GamePlayBackground"))
        {
            AudioManager.Instance.Stop("GamePlayBackground");
        }

        AudioManager.Instance.Play("MenuBackground");
    }

    public void Play()
    {
        sceneChanger.TransitionTo(sceneToLoad);
        Debug.Log("Starting game...");
    }
}