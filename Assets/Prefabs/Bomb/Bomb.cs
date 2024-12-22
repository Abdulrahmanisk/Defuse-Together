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
        [SerializeField] Button ClearButton;

        [Header("Settings")]
        [SerializeField] string[] correctCodes;
        [SerializeField] GameObject targetObjectToActivate; 

        string currentInput = "";
        int currentCodeIndex = 0; 

        void Awake()
        {
            foreach (var button in keypadButtons)
            {
                string key = button.GetComponentInChildren<TMP_Text>().text;
                button.onClick.AddListener(() => OnKeyPress(key));
            }
            enterButton.onClick.AddListener(OnEnterPress); 
        }

        void Start()
        {
            if (targetObjectToActivate != null)
            {
                targetObjectToActivate.SetActive(false); 
            }

            UpdateDisplay("Enter Code");  
        }

        void OnKeyPress(string key)
        {
            Debug.Log($"Key Pressed: {key}");

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
            string trimmedInput = currentInput.Trim();
            Debug.Log($"Current Input: {trimmedInput}");

            if (int.TryParse(trimmedInput, out int inputNumber))
            {
                Debug.Log($"Parsed Input: {inputNumber}");
                if (int.TryParse(correctCodes[currentCodeIndex], out int correctNumber))
                {
                    Debug.Log($"Correct Code: {correctNumber}");
                    if (inputNumber == correctNumber)
                    {
                        currentCodeIndex++;

                        if (currentCodeIndex >= correctCodes.Length)
                        {
                            GrantAccess(); 
                        }
                        else
                        {
                            UpdateDisplay($"Code {currentCodeIndex}Done Next Code");
                        }
                    }
                    else
                    {
                        UpdateDisplay("Access Denied");
                        StartCoroutine(ClearDisplayAfterDelay());
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
                UpdateDisplay("Invalid Input");
                StartCoroutine(ClearDisplayAfterDelay());
            }

            currentInput = "";
        }

        void GrantAccess()
        {
            UpdateDisplay("Access Granted");

            if (targetObjectToActivate != null)
            {
                targetObjectToActivate.SetActive(true);
            }
        }

        void UpdateDisplay(string text)
        {
            displayText.text = text;
        }

        IEnumerator ClearDisplayAfterDelay()
        {
            yield return new WaitForSeconds(1.5f);
            UpdateDisplay($"Enter Code {currentCodeIndex + 1}");
        }

        public void SubmitTextLocal(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)) return;

            if (currentInput.Length <= 4)
            {
                currentInput += text;
                UpdateDisplay(currentInput);
            }
            
        }public void Clear()
            {
            UpdateDisplay("Cleared!!");
            }
    }
}