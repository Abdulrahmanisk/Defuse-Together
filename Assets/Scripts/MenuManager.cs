using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scenetransaction;

public class MenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject main;
    [SerializeField] GameObject howToPlay;

    [Header("SceneName")]
    [SerializeField] string sceneName;
    [Header("SceneManager")]
    [SerializeField]SceneTransition scene;
    
    void Start()
    {
        main.SetActive(true);
        howToPlay.SetActive(false);
    }

    public void HowtoPlay() {
        main.SetActive(false);
        howToPlay.SetActive(true);
    }

    public void Back()
    {
        howToPlay.SetActive(false);
        main.SetActive(true);
    }

    public void Play()
    {

        scene.LoadScene(sceneName);
    }
}

