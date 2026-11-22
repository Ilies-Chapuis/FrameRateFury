using UnityEngine;

public class PerformanceCounter : MonoBehaviour
{
    public static PerformanceCounter Instance { get; private set; }

    public float Fps { get; private set; }
    public int ActiveEntities { get; private set; }

    [SerializeField] private float refreshInterval = 0.5f;

    private float timer;
    private int frames;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        frames++;
        timer += Time.unscaledDeltaTime;

        if (timer >= refreshInterval)
        {
            Fps = frames / timer;
            frames = 0;
            timer = 0f;
        }
    }

    public void Register() => ActiveEntities++;
    public void Unregister() => ActiveEntities--;
}