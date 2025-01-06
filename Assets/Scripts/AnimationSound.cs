using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip soundClip; // D�wi�k, kt�ry chcemy odtworzy�
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Pobranie komponentu AudioSource
    }

    // Ta metoda b�dzie wywo�ywana przez event z animacji
    public void PlaySoundFromAnimation()
    {
        if (audioSource != null && soundClip != null)
        {
            audioSource.PlayOneShot(soundClip); // Odtwarzanie d�wi�ku
        }
    }
}
