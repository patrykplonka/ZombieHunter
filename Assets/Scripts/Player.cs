using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] float moveSpeed = 6;
    [SerializeField] WaveManager waveManager;  // Odniesienie do WaveManager


    Animator anim;
    Rigidbody2D rb;

    int maxHealth = 100;
    int currentHealth;

    bool dead = false;

    float moveHorizontal, moveVertical;
    Vector2 movement;

    int facingDirection = 1;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        UpdateHealthText(); // Wywołaj metodę aktualizującą tekst HP
    }

    private void Update()
    {
        if (dead)
        {
            movement = Vector2.zero;
            anim.SetFloat("velocity", 0);
            return;
        }

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        anim.SetFloat("velocity", movement.magnitude);

        if (movement.x != 0)
            facingDirection = movement.x > 0 ? 1 : -1;

        transform.localScale = new Vector2(facingDirection, 1);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
            Hit(20);
    }

   private void OnTriggerEnter2D(Collider2D other)
{
    Medkit medkit = other.GetComponent<Medkit>();

    if (medkit != null)
    {
        IncreaseHealth(medkit.GetHealAmount()); // Dodaj zdrowie z medkitu
        medkit.Pickup(); // Uruchom logikę zebrania w skrypcie Medkit
    }
}


    void Hit(int damage)
    {
        anim.SetTrigger("hit");
        currentHealth -= damage;
        UpdateHealthText();

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        dead = true;
        if (waveManager != null)
        {
            waveManager.OnPlayerDeath();  // Powiadom WaveManager o śmierci gracza
        }
        GameManager.Instance.GameOver();
    }


    public void IncreaseHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString();
    }

    public int GetCurrentHealth() // Nowa metoda
    {
        return currentHealth;
    }
}
