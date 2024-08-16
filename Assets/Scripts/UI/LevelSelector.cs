using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public SceneChanger sceneChanger;

    private void Start()
    {
        if (AudioManager.Instance.IsPlaying("GamePlayBackground"))
        {
            AudioManager.Instance.Stop("GamePlayBackground");
            AudioManager.Instance.Play("MenuBackground");
        }
    }

    public void SelectLevel(string levelName)
    {
        sceneChanger.TransitionTo(levelName);
    }
}