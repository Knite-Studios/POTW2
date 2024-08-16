using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public Image transitionImage;
    public float transitionDuration = 1f;

    private void Start()
    {
        if (transitionImage != null)
        {
            StartCoroutine(TransitionIn());
        }
        else
        {
            Debug.LogWarning("Transition image not assigned in SceneChanger.");
        }
    }

    public void TransitionTo(string sceneName)
    {
        if (transitionImage != null)
        {
            StartCoroutine(TransitionOut(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private IEnumerator TransitionIn()
    {
        float elapsedTime = transitionDuration;
        while (elapsedTime > 0f)
        {
            elapsedTime -= Time.deltaTime;
            transitionImage.color = new Color(0f, 0f, 0f, elapsedTime / transitionDuration);
            yield return null;
        }
    }

    private IEnumerator TransitionOut(string sceneName)
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            transitionImage.color = new Color(0f, 0f, 0f, elapsedTime / transitionDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}