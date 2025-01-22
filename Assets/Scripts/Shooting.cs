using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;    // Prefab pocisku
    [SerializeField] private Transform shootingPoint;    // Punkt, z którego strzelają pociski
    [SerializeField] private float fireRate = 0.5f;      // Czas między strzałami (szybkość strzelania)
    private float nextFireTime = 0f;                     // Czas, kiedy gracz może strzelić ponownie

    void Update()
    {
        // Sprawdzamy, czy czas na strzał (na podstawie fireRate) i czy przycisk strzału jest wciśnięty
        if (Time.time > nextFireTime)
        {
            if (Input.GetButton("Fire1"))  // Fire1 to domyślny przycisk (np. lewy przycisk myszy)
            {
                Fire();
                nextFireTime = Time.time + 1f / fireRate;  // Ustawiamy czas, kiedy gracz może strzelić ponownie
            }
        }
    }

    // Ustawienie szybkości strzelania (fireRate)
    public void SetFireRate(float newFireRate)
    {
        fireRate = newFireRate;
    }

    // Funkcja do wykonywania strzału
    private void Fire()
    {
        Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);  // Tworzymy pocisk w wyznaczonej pozycji
    }
}
