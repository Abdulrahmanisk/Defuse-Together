using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TheBomb
{
    public class Bomb : MonoBehaviour
    {
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
        [SerializeField] int maxWrongAttempts = 3; // عدد المحاولات الخاطئة قبل الانفجار
        [SerializeField] float timer = 30f; // الوقت قبل الانفجار
        [SerializeField] TMP_Text timerText; // النص لعرض الوقت المتبقي

        [Header("Audio Settings")]
        [SerializeField] AudioClip correctCodeSound;
        [SerializeField] AudioClip wrongCodeSound;
        [SerializeField] AudioClip explosionSound;
        [SerializeField] AudioSource audioSource;

        private string currentInput = "";
        private int currentCodeIndex = 0;
        private int wrongAttempts = 0; // عدد المحاولات الخاطئة
        private bool isExploded = false; // للتأكد أن الانفجار يحدث مرة واحدة

        void Awake()
        {
            foreach (var button in keypadButtons)
            {
                string key = button.GetComponentInChildren<TMP_Text>().text;
                button.onClick.AddListener(() => OnKeyPress(key));
            }
            enterButton.onClick.AddListener(OnEnterPress);
            clearButton.onClick.AddListener(Clear);
        }

        void Start()
        {
            if (targetObjectToActivate != null)
            {
                targetObjectToActivate.SetActive(false);
            }

            UpdateDisplay("Enter Code");
            UpdatePanelColor(Color.yellow);

            if (timerText != null)
            {
                UpdateTimerText();
            }

            StartCoroutine(CountdownTimer());
        }

        void OnKeyPress(string key)
        {
            if (isExploded) return;

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

            if (int.TryParse(trimmedInput, out int inputNumber))
            {
                if (int.TryParse(correctCodes[currentCodeIndex], out int correctNumber))
                {
                    if (inputNumber == correctNumber)
                    {
                        PlaySound(correctCodeSound);
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
                        HandleWrongCode();
                    }
                }
                else
                {
                    Debug.LogError($"Error: Correct code at index {currentCodeIndex} is not a valid number!");
                }
            }
            else
            {
                Debug.LogError($"Invalid Input: {trimmedInput} is not a valid number.");
                HandleWrongCode();
            }

            currentInput = "";
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

            StartCoroutine(RestartSceneAfterDelay());
        }

        IEnumerator RestartSceneAfterDelay()
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        IEnumerator CountdownTimer()
        {
            while (timer > 0 && !isExploded)
            {
                timer -= Time.deltaTime;
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
                timerText.text = $"Time: {Mathf.Ceil(timer)}s";
            }
        }

        void GrantAccess()
        {
            UpdateDisplay("Access Granted");
            UpdatePanelColor(Color.green);

            if (targetObjectToActivate != null)
            {
                targetObjectToActivate.SetActive(true);
            }
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

        public void SubmitTextLocal(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)) return;

            if (currentInput.Length <= 4)
            {
                currentInput += text;
                UpdateDisplay(currentInput);
            }
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