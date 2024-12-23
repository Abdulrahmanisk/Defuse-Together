using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
namespace Scenetransaction
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 1f;

        private void Start()
        {
            if (fadeImage != null)
            {
                StartCoroutine(FadeIn());
            }
        }

        public void LoadScene(string sceneName)
        {
            if (fadeImage != null)
            {
                StartCoroutine(FadeOutAndLoad(sceneName));
            }
        }

        private IEnumerator FadeIn()
        {
            float timer = fadeDuration;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                float alpha = Mathf.Clamp01(timer / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
            fadeImage.raycastTarget = false;
        }

        private IEnumerator FadeOutAndLoad(string sceneName)
        {
            fadeImage.raycastTarget = true;
            float timer = 0;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(timer / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}