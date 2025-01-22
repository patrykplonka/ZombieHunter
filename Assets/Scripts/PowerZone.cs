using UnityEngine;

public class PowerZone : MonoBehaviour
{
    [Header("Power Up Settings")]
    [SerializeField] float speedMultiplier = 1.5f; // Mnożnik prędkości
    [SerializeField] float fireRateMultiplier = 4.5f; // Mnożnik szybkości strzelania (np. 0.5x szybsze)

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzamy, czy w strefie jest gracz
        if (other.CompareTag("Player"))
        {
            // Zwiększamy prędkość gracza i szybkość strzałów
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.SetPowerUp(true, speedMultiplier, fireRateMultiplier);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Sprawdzamy, czy gracz wychodzi ze strefy
        if (other.CompareTag("Player"))
        {
            // Przywracamy normalne wartości
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.SetPowerUp(false, 1f, 1f); // Przywracamy wartości bazowe
            }
        }
    }
}
