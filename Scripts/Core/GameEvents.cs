using System;

public static class GameEvents
{
    public static event Action<int> ScoreChanged;          // nouveau score
    public static event Action<int, int> HealthChanged;    // PV actuels, PV max
    public static event Action<int> WaveChanged;           // numéro de vague
    public static event Action<bool> PauseChanged;         // true = en pause
    public static event Action<int> GameOver;              // score final

    public static void RaiseScoreChanged(int score) => ScoreChanged?.Invoke(score);
    public static void RaiseHealthChanged(int current, int max) => HealthChanged?.Invoke(current, max);
    public static void RaiseWaveChanged(int wave) => WaveChanged?.Invoke(wave);
    public static void RaisePauseChanged(bool paused) => PauseChanged?.Invoke(paused);
    public static void RaiseGameOver(int finalScore) => GameOver?.Invoke(finalScore);
}