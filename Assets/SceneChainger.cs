using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChainger : MonoBehaviour
{
    public void Mainmenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Retray()
    {
        SceneManager.LoadScene("GameScene");
    }
}
