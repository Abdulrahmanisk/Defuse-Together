using UnityEngine;

public class SpotlightFlicker : MonoBehaviour
{
    public Light spotlight; // Assign the spotlight here.
    public AudioClip onSound; // Sound to play when the light turns on.
    public AudioClip offSound; // Sound to play when the light turns off.
    public AudioSource audioSource; // Audio source to play the sounds.

    public float minInterval = 0.05f; // Minimum time between flickers.
    public float maxInterval = 0.5f; // Maximum time between flickers.

    public float longOffChance = 0f; // Chance for a longer off period (0.0 to 1.0).
    public float longOnChance = 0.2f;  // Chance for a longer on period (0.0 to 1.0).
    public float longDuration = 1.5f;  // Duration for long on/off periods.

    private float nextFlickerTime;

    void Start()
    {
        if (spotlight == null)
        {
            spotlight = GetComponent<Light>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        SetNextFlickerTime();
    }

    void Update()
    {
        if (Time.time >= nextFlickerTime)
        {
            // Simulate faulty lamp behavior
            if (Random.value < longOffChance && spotlight.enabled)
            {
                PlaySound(offSound);
                spotlight.enabled = false;
                nextFlickerTime = Time.time + longDuration;
                return;
            }

            if (Random.value < longOnChance && !spotlight.enabled)
            {
                PlaySound(onSound);
                spotlight.enabled = true;
                nextFlickerTime = Time.time + longDuration;
                return;
            }

            // Toggle the spotlight on and off
            spotlight.enabled = !spotlight.enabled;
            PlaySound(spotlight.enabled ? onSound : offSound);

            // Set the next flicker time
            SetNextFlickerTime();
        }
    }

    void SetNextFlickerTime()
    {
        nextFlickerTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
