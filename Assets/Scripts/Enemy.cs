
using UnityEngine;
using System.Collections;
using Unity.Mathematics; // Upewnij się, że ta przestrzeń nazw jest dodana

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] float speed = 2f;
    [SerializeField] AudioClip hitSound; // Dźwięk trafienia
    [SerializeField] float volume = 1f; // Głośność dźwięku

    [Header("Death")]
    [SerializeField] AudioClip deathSound; // Dźwięk śmierci

    [Header("Charger")]
    [SerializeField] bool isCharger;
    [SerializeField] float distanceToCharge = 5f;
    [SerializeField] float chargeSpeed = 12f;
    [SerializeField] float prepareTime = 2f;
    [Header("Score")]
    [SerializeField] public int points = 1; // Punkty za zabicie tego przeciwnika


    bool isCharging = false;
    bool isPreparingCharge = false;

    public int MaxHealth => maxHealth;
public float Speed => speed;


    private int currentHealth;
    private Animator anim;
    private Transform target; // follow target
    private AudioSource audioSource; // AudioSource do odtwarzania dźwięków
    private SpriteRenderer spriteRenderer; // Komponent SpriteRenderer do zmiany koloru

 private void Start()
{
    currentHealth = maxHealth;

    // Check if the player exists
    GameObject player = GameObject.Find("Player");
    if (player != null)
    {
        target = player.transform;
    }
    else
    {
        Debug.LogError("Player object not found in the scene!");
    }

    anim = GetComponent<Animator>();
    if (anim == null)
    {
        Debug.LogError("Animator component missing on the Enemy object.");
    }

    audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    spriteRenderer = GetComponent<SpriteRenderer>();
    if (spriteRenderer == null)
    {
        Debug.LogError("SpriteRenderer component missing on the Enemy object.");
    }

    // Fetch stats based on difficulty, passing baseHealth and baseSpeed
    var stats = DifficultyManager.Instance.GetEnemyStats(maxHealth, speed);
    SetStats(stats.health, stats.speed);
}



    private void Update()
    {
        if (!WaveManager.Instance.WaveRunning()) return;
        if (isPreparingCharge) return;

        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;

            var playerToTheRight = target.position.x > transform.position.x;
            transform.localScale = new Vector2(playerToTheRight ? 1 : -1, 1);

            if (isCharger && !isCharging && Vector2.Distance(transform.position, target.position) < distanceToCharge)
            {
                isPreparingCharge = true;
                Invoke("StartCharging", prepareTime);
            }
        }
    }

    void StartCharging()
    {
        isPreparingCharge = false;
        isCharging = true;
        speed = chargeSpeed;
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("hit");

        // Efekt migania
        StartCoroutine(FlashRed());

        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound, volume); // Odtwarzaj dźwięk trafienia
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < 3; i++) // Liczba mignięć
        {
            spriteRenderer.color = Color.red; // Ustaw kolor na czerwony
            yield return new WaitForSeconds(0.01f); // Krótki czas migania
            spriteRenderer.color = originalColor; // Powrót do oryginalnego koloru
            yield return new WaitForSeconds(0.01f); // Równy czas powrotu
        }
    }

    private void Die()
{
    anim.SetTrigger("die");

    if (deathSound != null && audioSource != null)
    {
        audioSource.PlayOneShot(deathSound, volume);
    }

    // Dodanie punktów do wyniku w WaveManager
    WaveManager.Instance?.AddScore(points);

    Destroy(gameObject, deathSound != null ? deathSound.length : 0f);
}


    public void SetStats(int newMaxHealth, float newSpeed)
    {
        maxHealth = newMaxHealth;
        speed = newSpeed;

        // Jeżeli HP jest większe niż bieżące, to przywracamy pełne HP
        currentHealth = maxHealth;
    }
}
