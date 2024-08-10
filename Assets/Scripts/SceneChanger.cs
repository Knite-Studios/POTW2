using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public Image image;

    private void Start()
    {
        StartCoroutine(trasnitionIn());
    }

    public void transitionTo(string scene)
    {
        Debug.Log("test2");
        StartCoroutine(trasnitionOut(scene));
    }

    private IEnumerator trasnitionIn()
    {
        var t = 1f;
        Debug.Log("test3");
        while (t > 0f)
        {
            t -= Time.deltaTime;
            image.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }
    }

    private IEnumerator trasnitionOut(string scene)
    {
        var t = 0f;
        Debug.Log("test4");
        while (t < 1f)
        {
            t += Time.deltaTime;
            image.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }

        SceneManager.LoadScene(scene);
        Debug.Log("test5");
    }
}