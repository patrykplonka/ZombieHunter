using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Button restartButton;
    [SerializeField] AudioClip gameOverSound; 
    [SerializeField] float volume = 1f; 
    private bool gameRunning;
    private AudioSource audioSource; 

    public static GameManager Instance; // Zmieniłem instance na Instance

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Zapewnia istnienie jednej instancji po załadowaniu nowej sceny.
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        gameRunning = true;  // Na początku gra jest uruchomiona
    }

    void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        gameRunning = true; // Ustawia, że gra znowu działa
        gameOverPanel.SetActive(false); // Ukryj panel przegranej
    }
    
    public bool IsGameRunning()
    {
        return gameRunning;
    }

    public void GameOver()
    {
        gameRunning = false;
        gameOverPanel.SetActive(true);

        if (gameOverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gameOverSound, volume); 
        }
        else
        {
            Debug.LogError("Missing gameOverSound or audioSource.");
        }

        Time.timeScale = 0;
    }

    
}