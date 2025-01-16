using System.Collections;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] TextMeshProUGUI scoreText; // Pole tekstowe do wyświetlania wyniku
    [SerializeField] Player player; // Zaktualizuj odniesienie do gracza

    public static WaveManager Instance;

    public delegate void WaveChanged(int waveNumber);
    public event WaveChanged OnWaveChanged;

    public delegate void DifficultyIncreased(float healthMultiplier, float speedMultiplier);
    public event DifficultyIncreased OnDifficultyIncreased;

    bool waveRunning = true;
    int currentWave = 0;
    int currentWaveTime;
    int score = 0; // Zmienna przechowująca wynik

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        StartNewWave();
        UpdateScoreText(); // Zaktualizuj wynik na starcie
    }

    public bool WaveRunning() => waveRunning;

    public bool IsWavePaused() => !waveRunning;

    private void StartNewWave()
    {
        StopAllCoroutines();
        timeText.color = Color.white;
        currentWave++;

        currentWaveTime = 8 + (currentWave - 1) * 10;

        waveRunning = true;
        waveText.text = "Wave: " + currentWave;
        StartCoroutine(WaveTimer());

        OnWaveChanged?.Invoke(currentWave);

        if (currentWave >= 6)
        {
            float healthMultiplier = 1 + (currentWave - 5) * 0.1f;
            float speedMultiplier = 1 + (currentWave - 5) * 0.05f;
            OnDifficultyIncreased?.Invoke(healthMultiplier, speedMultiplier);
        }

        if (player != null)
        {
            player.IncreaseHealth(50);
        }
    }

    IEnumerator WaveTimer()
    {
        while (waveRunning)
        {
            yield return new WaitForSeconds(1f);
            currentWaveTime--;
            timeText.text = currentWaveTime.ToString();
            if (currentWaveTime <= 0) WaveComplete();
        }
    }

    private void WaveComplete()
    {
        StopAllCoroutines();
        EnemyManager.Instance.DestroyAllEnemies();
        waveRunning = false;

        waveText.text = "Wave #" + currentWave + " Complete!";

        timeText.color = Color.red;
        currentWaveTime = 5;
        StartCoroutine(WaitAndStartNextWave());

        OnWaveChanged?.Invoke(currentWave);
    }

    IEnumerator WaitAndStartNextWave()
    {
        while (currentWaveTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentWaveTime--;
            timeText.text = currentWaveTime.ToString();
        }

        StartNewWave();
    }

    // Dodanie metody do aktualizacji wyniku
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // Zwracanie wyniku (np. do wyświetlenia po śmierci)
    public int GetScore() => score;
    public void OnPlayerDeath()
    {
        // Wyświetl wynik po śmierci gracza
        waveRunning = false;
        timeText.color = Color.red;
        waveText.text = "Game Over!";

        // Wyświetl wynik
        scoreText.text = "Final Score: " + score;

        // Możesz dodać kod, który np. wyświetli przycisk restartu gry
    }

}
