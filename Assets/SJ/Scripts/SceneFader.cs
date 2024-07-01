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

        // ���̵� �ƿ�
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        // �� �ε�
        SceneManager.LoadScene(sceneName);

        // �� �ε� �� ��ٸ���
        yield return new WaitForSeconds(0.1f);

        // ���̵� ��
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeIn());
        }

        isFading = false;
    }

    private IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true); // ������ �̹��� Ȱ��ȭ

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

        fadeImage.gameObject.SetActive(false); // ������ �̹��� ��Ȱ��ȭ
    }
}
