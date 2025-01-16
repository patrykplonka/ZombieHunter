using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lvlText; // Pole tekstowe dla poziomu trudności
    [SerializeField] private Button easyButton;       // Przycisk dla poziomu Easy
    [SerializeField] private Button normalButton;     // Przycisk dla poziomu Normal
    [SerializeField] private Button hardButton;       // Przycisk dla poziomu Hard

    private void Start()
    {
        // Przypisanie funkcji do przycisków
        easyButton.onClick.AddListener(() => SetDifficulty("Easy"));
        normalButton.onClick.AddListener(() => SetDifficulty("Normal"));
        hardButton.onClick.AddListener(() => SetDifficulty("Hard"));

        // Odczyt zapisanej trudności przy ponownym uruchomieniu sceny
        LoadDifficulty();
    }

    public void SetDifficulty(string difficulty)
    {
        // Zapisanie poziomu trudności w PlayerPrefs
        PlayerPrefs.SetString("Difficulty", difficulty);
        PlayerPrefs.Save();

        // Sprawdzenie, czy DifficultyManager istnieje
        if (DifficultyManager.Instance == null)
        {
            Debug.LogError("DifficultyManager.Instance is null. Make sure it is initialized.");
            return;
        }

        // Ustawienie poziomu trudności w DifficultyManager
        if (System.Enum.TryParse(difficulty, out DifficultyManager.DifficultyLevel parsedDifficulty))
        {
            DifficultyManager.Instance.SetDifficulty(parsedDifficulty);
        }

        // Ustawienie poziomu trudności na ekranie
        UpdateDifficultyText(difficulty);
    }

    private void LoadDifficulty()
    {
        // Odczytanie zapisanego poziomu trudności
        string savedDifficulty = PlayerPrefs.GetString("Difficulty", "Normal"); // Domyślnie "Normal" jeśli brak zapisanego
        UpdateDifficultyText(savedDifficulty);
    }

    private void UpdateDifficultyText(string difficulty)
    {
        // Ustawienie tekstu i koloru dla poziomu trudności
        switch (difficulty)
        {
            case "Easy":
                lvlText.text = "Easy";
                lvlText.color = Color.green;
                break;
            case "Normal":
                lvlText.text = "Normal";
                lvlText.color = Color.yellow;
                break;
            case "Hard":
                lvlText.text = "Hard";
                lvlText.color = Color.red;
                break;
            default:
                lvlText.text = "Unknown";
                lvlText.color = Color.white;
                break;
        }
    }
}
