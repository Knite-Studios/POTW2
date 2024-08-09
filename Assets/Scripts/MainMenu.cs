using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public string sceneToLoad = "GamePlay";

    private void Start()
    {
        if (AudioManager.Instance.isPlaying("GamePlayBackground"))
        {
            AudioManager.Instance.stop("GamePlayBackground");
        }

        AudioManager.Instance.play("MenuBackground");
    }

    public void play()
    {
        sceneChanger.transitionTo(sceneToLoad);
        Debug.Log("test1");
    }
}