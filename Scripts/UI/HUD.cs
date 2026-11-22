using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("Jeu")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private Button pauseButton;

    [Header("Performance")]
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private TMP_Text entitiesText;
    [SerializeField] private float debugRefreshInterval = 0.5f;

    [Header("PV en cœurs")]
    [SerializeField] private Image[] hearts;   // 3 Images
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartHalf;
    [SerializeField] private Sprite heartEmpty;

    private float timer;

    void OnEnable()
    {
        GameEvents.ScoreChanged += OnScoreChanged;
        GameEvents.HealthChanged += OnHealthChanged;
        GameEvents.WaveChanged += OnWaveChanged;
        pauseButton.onClick.AddListener(OnPauseClicked);
    }

    void OnDisable()
    {
        GameEvents.ScoreChanged -= OnScoreChanged;
        GameEvents.HealthChanged -= OnHealthChanged;
        GameEvents.WaveChanged -= OnWaveChanged;
        pauseButton.onClick.RemoveListener(OnPauseClicked);
    }

    void Start()
    {
        // Valeurs de départ, en attendant les premiers événements
        OnScoreChanged(0);
        OnWaveChanged(1);
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer < debugRefreshInterval) return;
        timer = 0f;

        if (PerformanceCounter.Instance == null) return;
        fpsText.text = $"FPS : {PerformanceCounter.Instance.Fps:F0}";
        entitiesText.text = $"Entités : {PerformanceCounter.Instance.ActiveEntities}";
    }

    private void OnScoreChanged(int score) => scoreText.text = $"Score : {score}";
    private void OnHealthChanged(int current, int max)
    {
        // Mettre à jour les cœurs de vie
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartValue = current - i * 2;   // PV restants pour ce cœur

            if (heartValue >= 2)      hearts[i].sprite = heartFull;
            else if (heartValue == 1) hearts[i].sprite = heartHalf;
            else                      hearts[i].sprite = heartEmpty;

            hearts[i].enabled = i * 2 < max;
        }
    }
    private void OnWaveChanged(int wave) => waveText.text = $"Vague {wave}";
    private void OnPauseClicked() => GameEvents.RaisePauseChanged(true);
}