using UnityEngine;
using UnityEngine.UI;

public class WaveBar : MonoBehaviour
{
    public static WaveBar Instance { get; private set; }

    [SerializeField] private Slider waveSlider;
    public bool isPaused = true;
    
    private float countDown;
    private float time;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one wave bar manager in the scene!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (countDown < time)
            {
                countDown += Time.deltaTime;
                waveSlider.value = countDown;
            }
        }
    }

    public void startBar(float time)
    {
        this.time = time;
        waveSlider.maxValue = time;
        countDown = 0f;
        waveSlider.value = countDown;
        isPaused = false;
    }

    public void pause()
    {
        isPaused = true;
    }

    public void resume()
    {
        isPaused = false;
    }
}