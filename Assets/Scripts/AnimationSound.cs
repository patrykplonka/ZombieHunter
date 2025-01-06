using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip soundClip; // DŸwiêk, który chcemy odtworzyæ
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Pobranie komponentu AudioSource
    }

    // Ta metoda bêdzie wywo³ywana przez event z animacji
    public void PlaySoundFromAnimation()
    {
        if (audioSource != null && soundClip != null)
        {
            audioSource.PlayOneShot(soundClip); // Odtwarzanie dŸwiêku
        }
    }
}
