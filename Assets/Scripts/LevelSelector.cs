using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public SceneChanger sceneChanger;

    private void Start()
    {
        if (AudioManager.Instance.isPlaying("GamePlayBackground"))
        {
            AudioManager.Instance.stop("GamePlayBackground");
            AudioManager.Instance.play("MenuBackground");
        }
    }

    public void selectLevel(string levelName)
    {
        sceneChanger.transitionTo(levelName);
    }
}