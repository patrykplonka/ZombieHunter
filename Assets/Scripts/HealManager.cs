using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealManager : MonoBehaviour
{
    [SerializeField] private GameObject medkitPrefab; // Prefab medkitu
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-13f, -8f); // Minimalne współrzędne
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(13f, 8f);   // Maksymalne współrzędne
    [SerializeField] private float respawnInterval = 5f; // Interwał w sekundach
    [SerializeField] private float minSpawnDistance = 3f; // Minimalna odległość między medkitami

    private Transform medkitsParent;
    private List<Medkit> activeMedkits = new List<Medkit>(); // Lista aktywnych medkitów

    private void Start()
    {
        // Tworzymy kontener dla medkitów
        medkitsParent = new GameObject("Medkits").transform;

        // Subskrybujemy na zdarzenie końca fali z WaveManager
        WaveManager.Instance.OnWaveChanged += OnWaveChanged;

        StartCoroutine(SpawnMedkits());
    }

    private void OnWaveChanged(int waveNumber)
    {
        // Natychmiastowe usuwanie medkitów po zakończeniu fali
        foreach (var medkit in activeMedkits)
        {
            Destroy(medkit.gameObject); // Usuwamy wszystkie medkity
        }
        activeMedkits.Clear(); // Czyścimy listę medkitów
    }

    private IEnumerator SpawnMedkits()
    {
        while (true)
        {
            if (WaveManager.Instance.WaveRunning()) // Tylko jeśli fala jest aktywna
            {
                if (activeMedkits.Count < 5) // Jeśli liczba medkitów na ziemi jest mniejsza niż 5
                {
                    SpawnMedkit(); // Spawnujemy medkit
                }
            }
            yield return new WaitForSeconds(respawnInterval); // Czekamy przed kolejnym respawnem
        }
    }

    private void SpawnMedkit()
    {
        Vector2 spawnPosition = GetValidSpawnPosition();

        // Tworzymy medkit na wylosowanej pozycji
        GameObject medkitObject = Instantiate(medkitPrefab, spawnPosition, Quaternion.identity, medkitsParent);
        Medkit medkit = medkitObject.GetComponent<Medkit>();

        // Dodajemy medkit do listy aktywnych medkitów
        activeMedkits.Add(medkit);

        // Subskrybujemy na zdarzenie, które zaktualizuje listę, gdy medkit zostanie zebrany
        medkit.OnMedkitCollected += () => OnMedkitCollected(medkit);
    }

    private Vector2 GetValidSpawnPosition()
    {
        Vector2 spawnPosition;
        bool positionIsValid = false;

        // Pętla, aby próbować znaleźć poprawną pozycję
        do
        {
            spawnPosition = RandomPosition(); // Losujemy pozycję
            positionIsValid = true;

            // Sprawdzamy odległość od wszystkich medkitów
            foreach (var medkit in activeMedkits)
            {
                if (Vector2.Distance(spawnPosition, medkit.transform.position) < minSpawnDistance)
                {
                    positionIsValid = false; // Pozycja jest zbyt blisko innego medkitu
                    break;
                }
            }
        }
        while (!positionIsValid); // Powtarzamy, dopóki nie znajdziemy odpowiedniej pozycji

        return spawnPosition; // Zwracamy ważną pozycję
    }

    private void OnMedkitCollected(Medkit medkit)
    {
        // Usuwamy medkit z listy po zebraniu
        activeMedkits.Remove(medkit);
    }

    private Vector2 RandomPosition()
    {
        // Losowanie pozycji w zadanym obszarze
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }
}
