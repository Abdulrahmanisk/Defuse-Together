using UnityEngine;

public class MenuLight : MonoBehaviour
{
    public Light spotlight; 
    public AudioClip onSound; 
    public AudioClip offSound; 
    public AudioSource audioSource; 
    void Start()
    {
        spotlight.enabled = false;

        if (spotlight == null)
        {
            spotlight = GetComponent<Light>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

    }


    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }

    public void EnableLight()
    {
        PlaySound(onSound);
        spotlight.enabled = true;
    }
    public void DesableLight()
    {
        PlaySound(offSound);
        spotlight.enabled = false;
    }
}
