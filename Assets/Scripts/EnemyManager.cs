
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;     // Typ 1 przeciwnika
    [SerializeField] GameObject chargerPrefab;  // Typ 2 przeciwnika
    [SerializeField] GameObject enemy2Prefab;   // Typ 3 przeciwnika
    [SerializeField] GameObject enemy3Prefab;   // Typ 4 przeciwnika
    [SerializeField] GameObject enemy4Prefab;   // Typ 5 przeciwnika
    [SerializeField] float timeBetweenSpawns = 0.5f; // Czas pomiędzy spawnami

    [SerializeField] Vector2 spawnAreaMin = new Vector2(-16, -8); // Lewy dolny róg prostokąta
    [SerializeField] Vector2 spawnAreaMax = new Vector2(16, 8);   // Prawy górny róg prostokąta

    float currentTimeBetweenSpawns;
    Transform enemiesParent;
    private float healthMultiplier = 1f;
    private float speedMultiplier = 1f;


    public static EnemyManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
{
    enemiesParent = GameObject.Find("Enemies").transform;

    // Zarejestruj się na zdarzenie zmiany trudności
    WaveManager.Instance.OnDifficultyIncreased += UpdateDifficulty;
}

// Metoda aktualizująca mnożniki trudności
private void UpdateDifficulty(float newHealthMultiplier, float newSpeedMultiplier)
{
    healthMultiplier = newHealthMultiplier;
    speedMultiplier = newSpeedMultiplier;
}


    private void Update()
    {
        if (!WaveManager.Instance.WaveRunning()) return;

        currentTimeBetweenSpawns -= Time.deltaTime;

        if (currentTimeBetweenSpawns <= 0)
        {
            SpawnEnemy();
            currentTimeBetweenSpawns = timeBetweenSpawns;
        }
    }

    Vector2 RandomPosition()
    {
        // Losuj pozycję wewnątrz prostokąta
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

        return new Vector2(x, y);
    }

   void SpawnEnemy()
{
    var roll = Random.Range(0, 10); // Losujemy liczbę od 0 do 9

    GameObject enemyType;

    // Decydujemy, jaki typ przeciwnika stworzyć
   if (roll < 3) // 30% szans na enemyPrefab
{
    enemyType = enemyPrefab;
    enemyPrefab.GetComponent<Enemy>().points = 1; // 1 punkt
}
else if (roll < 4) // 10% szans na chargerPrefab
{
    enemyType = chargerPrefab;
    chargerPrefab.GetComponent<Enemy>().points = 5; // 5 punktów
}
else if (roll < 6) // 20% szans na enemy2Prefab
{
    enemyType = enemy2Prefab;
    enemy2Prefab.GetComponent<Enemy>().points = 2; // 2 punkty
}
else if (roll < 8) // 20% szans na enemy3Prefab
{
    enemyType = enemy3Prefab;
    enemy3Prefab.GetComponent<Enemy>().points = 2; // 2 punkty
}
else // 20% szans na enemy4Prefab
{
    enemyType = enemy4Prefab;
    enemy4Prefab.GetComponent<Enemy>().points = 4; // 4 punkty
}


    // Tworzymy przeciwnika
    var e = Instantiate(enemyType, RandomPosition(), Quaternion.identity);
    e.transform.SetParent(enemiesParent);

    // Aktualizujemy statystyki przeciwnika
    var enemyScript = e.GetComponent<Enemy>();
    if (enemyScript != null)
    {
        enemyScript.SetStats(
            Mathf.RoundToInt(enemyScript.MaxHealth * healthMultiplier),
            enemyScript.Speed * speedMultiplier
        );
    }
}


    public void DestroyAllEnemies()
    {
        foreach (Transform e in enemiesParent)
            Destroy(e.gameObject);
    }
}