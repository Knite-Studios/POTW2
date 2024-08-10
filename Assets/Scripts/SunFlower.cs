using UnityEngine;

public class SunFlower : MonoBehaviour
{
    public Canvas pointCanvas;
    public float pointDelay = 5f;
    private bool isShowing;
    private float timer;
    private void Start()
    {
        timer = pointDelay;
        isShowing = false;
        pointCanvas.enabled = false;
    }

    private void Update()
    {
        if (timer <= 0f)
        {
            timer = pointDelay;
            if (isShowing)
            {
                hidePoint();
            }
            else
            {
                showPoint();
            }
        }

        timer -= Time.deltaTime;
    }

    public void pointClicked()
    {
        Debug.Log("Point clicked");
        MoneyManager.instance.onPointClicked();
        timer = pointDelay;
        hidePoint();
    }

    public void showPoint()
    {
        isShowing = true;
        pointCanvas.enabled = true;
        Debug.Log("normal anim started");
        gameObject.GetComponent<Animator>().Play("Normal");
    }

    public void hidePoint()
    {
        isShowing = false;
        pointCanvas.enabled = false;
    }
}