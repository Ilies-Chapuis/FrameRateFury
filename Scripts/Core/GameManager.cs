using UnityEngine;

public enum GameState { Menu, Playing, Paused, GameOver, Victory }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Références")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Debug")]
    [Tooltip("Lance la partie dès l'entrée dans la scène (à décocher quand le menu appellera StartGame)")]
    [SerializeField] private bool startOnPlay = true;

    public GameState State { get; private set; } = GameState.Menu;
    public int Score { get; private set; }
    public int Wave { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        if (playerHealth != null) playerHealth.OnDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        if (playerHealth != null) playerHealth.OnDied -= HandlePlayerDied;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Time.timeScale = 1f; // évite de rester bloqué en pause après un rechargement de scène
        }
    }

    private void Start()
    {
        if (startOnPlay) StartGame();
    }

    public void StartGame()
    {
        Score = 0;
        Wave = 0;
        Time.timeScale = 1f;
        State = GameState.Playing;

        GameEvents.RaiseScoreChanged(Score);
        GameEvents.RaiseWaveChanged(Wave);
        GameEvents.RaisePauseChanged(false);
    }

    public void AddScore(int points)
    {
        if (State != GameState.Playing) return;
        Score += points;
        GameEvents.RaiseScoreChanged(Score);
    }

    public void SetWave(int wave)
    {
        Wave = wave;
        GameEvents.RaiseWaveChanged(Wave);
    }

    public void PauseGame()
    {
        if (State != GameState.Playing) return;
        State = GameState.Paused;
        Time.timeScale = 0f;
        GameEvents.RaisePauseChanged(true);
    }

    public void ResumeGame()
    {
        if (State != GameState.Paused) return;
        State = GameState.Playing;
        Time.timeScale = 1f;
        GameEvents.RaisePauseChanged(false);
    }

    public void TogglePause()
    {
        if (State == GameState.Playing) PauseGame();
        else if (State == GameState.Paused) ResumeGame();
    }

    // Appelé par le WaveManager quand le boss est vaincu
    public void Win() => EndGame(GameState.Victory);

    private void HandlePlayerDied() => EndGame(GameState.GameOver);

    private void EndGame(GameState endState)
    {
        if (State == GameState.GameOver || State == GameState.Victory) return;

        State = endState;
        Time.timeScale = 0f;
        // TODO : GameEvents n'a pas d'événement "Victoire"
        GameEvents.RaiseGameOver(Score);
    }

    [ContextMenu("Test : +100 points")]
    private void TestScore() => AddScore(100);
}
