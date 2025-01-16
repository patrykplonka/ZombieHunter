using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] GameObject gunPrefab;     // Pierwszy typ broni
    [SerializeField] GameObject gun2Prefab;   // Drugi typ broni
    [SerializeField] GameObject gun3Prefab;   // Trzeci typ broni
    Transform player;
    List<Vector2> gunPositions = new List<Vector2>();

    int spawnedGuns = 0;  // Liczba aktualnie rozmieszczonych broni
    int maxGuns = 6;      // Maksymalna liczba broni do rozmieszczenia

    // Zmienna do przechowywania numeru obecnej fali
    private int currentWave = 0;

    private void Start()
    {
        player = GameObject.Find("Player").transform;

        gunPositions.Add(new Vector2(-1f, 1f));
        gunPositions.Add(new Vector2(1f, 1f));

        gunPositions.Add(new Vector2(-1.2f, 0.2f));
        gunPositions.Add(new Vector2(1.2f, 0.2f));

        gunPositions.Add(new Vector2(-1f, -0.5f));
        gunPositions.Add(new Vector2(1f, -0.5f));

        // Początkowe dodanie dwóch broni
        AddGun();

        // Subskrybujemy się na zdarzenie zmiany fali
        WaveManager.Instance.OnWaveChanged += OnWaveChanged;
    }

    private void OnDestroy()
    {
        // Usuwamy subskrypcję przy zniszczeniu obiektu
        if (WaveManager.Instance != null)
            WaveManager.Instance.OnWaveChanged -= OnWaveChanged;
    }

   private void Update()
{
    // Sprawdzanie, czy trzeba dodać broń w zależności od numeru fali
    if (spawnedGuns < Mathf.Min(currentWave, maxGuns))
    {
        AddGunForWave(currentWave);
    }

    // Sprawdzanie, czy naciśnięto przycisk 'G' i dodanie broni
    if (Input.GetKeyDown(KeyCode.G))
    {
         AddGun(); // Możesz tu dodać inną logikę, jeśli chcesz dodać inną broń
    }
}


    // Reakcja na zmianę fali
    private void OnWaveChanged(int waveNumber)
    {
        currentWave = waveNumber;
        Debug.Log($"Current wave changed to: {currentWave}");
    }

    // Metoda wywoływana przez WaveManager
    public void AddGunForWave(int waveNumber)
    {
        // Liczba broni w zależności od fali
        int gunsToAdd = Mathf.Min(waveNumber, maxGuns) - spawnedGuns;

        for (int i = 0; i < gunsToAdd; i++)
        {
            if (spawnedGuns < maxGuns)
            {
                AddGun();
            }
        }
    }

    void AddGun()
    {
        // Zapewniamy, że nie przekroczymy dostępnych pozycji
        if (spawnedGuns < gunPositions.Count)
        {
            // Losowy wybór prefabów broni
            GameObject selectedGunPrefab;
            int randomChoice = Random.Range(0, 3); // Losuje liczbę od 0 do 2
            switch (randomChoice)
            {
                case 0:
                    selectedGunPrefab = gunPrefab;
                    break;
                case 1:
                    selectedGunPrefab = gun2Prefab;
                    break;
                case 2:
                    selectedGunPrefab = gun3Prefab;
                    break;
                default:
                    selectedGunPrefab = gunPrefab; // Dla bezpieczeństwa
                    break;
            }

            var pos = gunPositions[spawnedGuns];
            var newGun = Instantiate(selectedGunPrefab, pos, Quaternion.identity);
            newGun.GetComponent<Gun>().SetOffset(pos);
            spawnedGuns++;

            Debug.Log($"Added new gun ({selectedGunPrefab.name}) at position {pos}");
        }
    }
}
