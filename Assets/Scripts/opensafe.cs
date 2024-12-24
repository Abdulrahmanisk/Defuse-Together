using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opensafe : MonoBehaviour
{
   public Animator m;
    [Header("Audio Settings")]
   [SerializeField] AudioClip SafeOpenSound;
   [SerializeField] AudioSource audio;
   [SerializeField] GameObject note;


    public void SafeOpen()
    {
        audio.PlayOneShot(SafeOpenSound);
        m.SetBool("Isopen", true);
        note.SetActive(true);
    }
}
