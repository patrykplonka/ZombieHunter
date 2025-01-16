
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance; // Singleton dla łatwego dostępu

    public enum DifficultyLevel { Easy, Normal, Hard }
    public DifficultyLevel CurrentDifficulty { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Utrzymanie obiektu między scenami
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Odczyt poziomu trudności z PlayerPrefs
        LoadDifficulty();
    }

    public void SetDifficulty(DifficultyLevel difficulty)
    {
        CurrentDifficulty = difficulty;

        // Zapis w PlayerPrefs
        PlayerPrefs.SetString("Difficulty", difficulty.ToString());
        PlayerPrefs.Save();
    }

    public void LoadDifficulty()
    {
        // Odczyt poziomu trudności lub ustawienie domyślnego (Normal)
        string savedDifficulty = PlayerPrefs.GetString("Difficulty", "Normal"); // Domyślnie ustawiamy "Normal"

        if (System.Enum.TryParse(savedDifficulty, out DifficultyLevel difficulty))
        {
            CurrentDifficulty = difficulty;
        }
        else
        {
            CurrentDifficulty = DifficultyLevel.Normal; // Ustawiamy domyślnie na Normal, jeśli odczytanie nie powiodło się
        }
    }

   // Funkcja pomocnicza do ustawienia statystyk wrogów na podstawie poziomu trudności
public (int health, float speed) GetEnemyStats(int baseHealth, float baseSpeed)
{
    // Wartości procentowe modyfikatorów dla różnych poziomów trudności
    float healthModifier = 1f;
    float speedModifier = 1f;

    switch (CurrentDifficulty)
    {
        case DifficultyLevel.Easy:
            healthModifier = 0.5f; // Zdrowie zmniejszone o 50% dla poziomu Easy
            speedModifier = 0.75f; // Prędkość zmniejszona o 25% dla poziomu Easy
            break;
        case DifficultyLevel.Normal:
            healthModifier = 1f; // Zdrowie na poziomie bazowym
            speedModifier = 1f; // Prędkość na poziomie bazowym
            break;
        case DifficultyLevel.Hard:
            healthModifier = 1.5f; // Zdrowie zwiększone o 50% dla poziomu Hard
            speedModifier = 1.5f; // Prędkość zwiększona o 50% dla poziomu Hard
            break;
    }

    // Obliczanie statystyk na podstawie bazowych wartości i modyfikatorów
    int calculatedHealth = Mathf.RoundToInt(baseHealth * healthModifier);
    float calculatedSpeed = baseSpeed * speedModifier;

    return (calculatedHealth, calculatedSpeed);
}

}