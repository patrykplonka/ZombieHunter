using UnityEngine;

public class Medkit : MonoBehaviour
{
    [SerializeField] private int healAmount = 20; // Ilość zdrowia przywracana przez medkit
    [SerializeField] private AudioClip pickupSound; // Dźwięk zbierania medkitu
    [SerializeField] private float floatAmplitude = 0.5f; // Amplituda unoszenia
    [SerializeField] private float floatFrequency = 1f; // Częstotliwość unoszenia

    private Vector3 startPosition; // Początkowa pozycja obiektu
    private float floatTimer = 0f; // Licznik czasu dla animacji unoszenia

    // Zdarzenie powiadamiające, że medkit został zebrany
    public delegate void MedkitCollected();
    public event MedkitCollected OnMedkitCollected;

    private void Start()
    {
        startPosition = transform.position; // Zapamiętaj początkową pozycję
    }

    private void Update()
    {
        // Aktualizacja pozycji unoszenia
        floatTimer += Time.deltaTime * floatFrequency;
        float newY = startPosition.y + Mathf.Sin(floatTimer) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    public int GetHealAmount()
    {
        return healAmount; // Zwraca ilość zdrowia, którą przywraca medkit
    }

    // Metoda wywoływana, kiedy gracz zbiera medkit
    public void Pickup()
    {
        // Odtwarzanie dźwięku zebrania, jeśli jest przypisany
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // Wywołanie zdarzenia, które informuje, że medkit został zebrany
        OnMedkitCollected?.Invoke();

        // Zniszczenie medkitu
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Jeśli koliduje z graczem
        {
            Pickup(); // Wywołanie metody Pickup
        }
    }
}
