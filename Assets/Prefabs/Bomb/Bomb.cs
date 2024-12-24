using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TheBomb
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float lastClockSoundTime = 0f;
        [SerializeField] private float clockSoundInterval = 1f;
        [Header("UI Elements")]
        [SerializeField] TMP_Text displayText;
        [SerializeField] Button[] keypadButtons;
        [SerializeField] Button enterButton;
        [SerializeField] Button clearButton;
        [SerializeField] Image panel;

        [Header("Settings")]
        [SerializeField] string[] correctCodes;
        [SerializeField] GameObject targetObjectToActivate;

        [Header("Explosion Settings")]
        [SerializeField] ParticleSystem explosionEffect;
        [SerializeField] int maxWrongAttempts = 3;
        [SerializeField] float timer = 30f;
        [SerializeField] TMP_Text timerText;

        [Header("Audio Settings")]
        [SerializeField] AudioClip correctCodeSound;
        [SerializeField] AudioClip wrongCodeSound;
        [SerializeField] AudioClip explosionSound;
        [SerializeField] AudioClip buttonPressSound;
        [SerializeField] AudioClip clockSound;
        [SerializeField] AudioSource audioSource;

        [Header("Scene Settings")]
        [SerializeField] string winSceneName = "WinScene";
        [SerializeField] string loseSceneName = "LoseScene";

        private string currentInput = "";
        private int currentCodeIndex = 0;
        private int wrongAttempts = 0;
        private bool isExploded = false;

        void Awake()
        {
            enterButton.onClick.AddListener(OnEnterPress);
            clearButton.onClick.AddListener(Clear);
        }

        void Start()
        {
            UpdateDisplay("Enter Code");
            UpdatePanelColor(Color.yellow);

            if (timerText != null)
            {
                UpdateTimerText();
            }

            StartCoroutine(CountdownTimer());
        }

        public void OnKeyPress(string key)
        {
            if (isExploded) return;

            PlaySound(buttonPressSound);

            if (int.TryParse(key, out _))
            {
                if (currentInput.Length <= 4)
                {
                    currentInput += key;
                    UpdateDisplay(currentInput);
                }
            }
            else
            {
                Debug.LogError($"Invalid Key Pressed: {key} is not a valid number.");
            }
        }

        public void OnEnterPress()
        {
            if (isExploded) return;

            string trimmedInput = currentInput.Trim();
            string correctCode = correctCodes[currentCodeIndex].Trim();

            Debug.Log($"Trimmed Input: '{trimmedInput}'");
            Debug.Log($"Correct Code: '{correctCode}'");
            Debug.Log($"Current Index: {currentCodeIndex}");

            if (string.IsNullOrEmpty(trimmedInput))
            {
                Debug.LogError("Input is empty, skipping comparison.");
                return;
            }

            if (string.Equals(trimmedInput, correctCode, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Code is correct");
                PlaySound(correctCodeSound);

                currentInput = "";
                currentCodeIndex++;

                if (currentCodeIndex >= correctCodes.Length)
                {
                    GrantAccess();
                }
                else
                {
                    UpdateDisplay($"Code {currentCodeIndex} Done Next Code");
                    UpdatePanelColor(Color.green);
                }
            }
            else
            {
                Debug.LogError("Code is incorrect");
                HandleWrongCode();
                currentInput = "";
            }
        }

        void HandleWrongCode()
        {
            wrongAttempts++;
            PlaySound(wrongCodeSound);

            if (wrongAttempts >= maxWrongAttempts)
            {
                Explode();
            }
            else
            {
                UpdateDisplay("Access Denied");
                UpdatePanelColor(Color.red);
                StartCoroutine(ClearDisplayAfterDelay());
            }
        }

        void Explode()
        {
            isExploded = true;
            UpdateDisplay("BOOM!");
            UpdatePanelColor(Color.red);

            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                explosionEffect.Play();
            }

            PlaySound(explosionSound);

            foreach (var button in keypadButtons)
            {
                button.interactable = false;
            }
            enterButton.interactable = false;
            clearButton.interactable = false;

            StartCoroutine(LoadLoseSceneAfterDelay());
        }

        IEnumerator LoadLoseSceneAfterDelay()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(loseSceneName);
        }

        IEnumerator CountdownTimer()
        {
            while (timer > 0 && !isExploded)
            {
                timer -= Time.deltaTime;
                if (Time.time >= lastClockSoundTime + clockSoundInterval)
                {
                    PlaySound(clockSound);
                    lastClockSoundTime = Time.time;
                }

                UpdateTimerText();
                yield return null;
            }

            if (timer <= 0 && !isExploded)
            {
                Explode();
            }
        }

        void UpdateTimerText()
        {
            if (timerText != null)
            {
                timerText.text = $"Time: {Mathf.Ceil(timer)} s";
            }
            else
            {
                Debug.Log("Timer not assigned");
            }
        }

        void GrantAccess()
        {
            UpdateDisplay("Access Granted");
            UpdatePanelColor(Color.green);

            SceneManager.LoadScene(winSceneName);
        }

        void UpdateDisplay(string text)
        {
            displayText.text = text;
        }

        void UpdatePanelColor(Color color)
        {
            if (panel != null)
            {
                panel.color = color;
            }
        }

        IEnumerator ClearDisplayAfterDelay()
        {
            yield return new WaitForSeconds(1.5f);
            UpdateDisplay($"Enter Code {currentCodeIndex + 1}");
            UpdatePanelColor(Color.yellow);
        }

        public void Clear()
        {
            currentInput = "";
            UpdateDisplay("Cleared!!");
            UpdatePanelColor(Color.yellow);
        }

        void PlaySound(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}