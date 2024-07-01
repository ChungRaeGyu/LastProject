using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;

    private Image fadeImage;
    private float fadeSpeed;
    private bool isFading = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void FadeOutAndLoadScene(string sceneName, Image image, float speed)
    {
        if (instance != null)
        {
            instance.fadeImage = image;
            instance.fadeSpeed = speed;
            instance.StartCoroutine(instance.FadeOutAndLoad(sceneName));
        }
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        if (isFading)
            yield break;

        isFading = true;

        // 페이드 아웃
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        // 씬 로드
        SceneManager.LoadScene(sceneName);

        // 씬 로드 후 기다리기
        yield return new WaitForSeconds(0.1f);

        // 페이드 인
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeIn());
        }

        isFading = false;
    }

    private IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true); // 검정색 이미지 활성화

        Color color = fadeImage.color;
        while (fadeImage.color.a < 1.0f)
        {
            float newAlpha = fadeImage.color.a + Time.deltaTime * fadeSpeed;
            color.a = Mathf.Clamp01(newAlpha);
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        while (fadeImage.color.a > 0.0f)
        {
            float newAlpha = fadeImage.color.a - Time.deltaTime * fadeSpeed;
            color.a = Mathf.Clamp01(newAlpha);
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false); // 검정색 이미지 비활성화
    }
}
