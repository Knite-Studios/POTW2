using UnityEngine;
using UnityEngine.UI;

public class WaveBar : MonoBehaviour
{
    public static WaveBar Instance { get; private set; }

    [SerializeField] private Slider waveSlider;
    public bool IsPaused { get; private set; } = true;
    
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
        if (!IsPaused)
        {
            if (countDown < time)
            {
                countDown += Time.deltaTime;
                waveSlider.value = countDown;
            }
        }
    }

    public void StartBar(float duration)
    {
        time = duration;
        waveSlider.maxValue = duration;
        countDown = 0f;
        waveSlider.value = countDown;
        IsPaused = false;
    }

    public void Pause()
    {
        IsPaused = true;
    }

    public void Resume()
    {
        IsPaused = false;
    }
}