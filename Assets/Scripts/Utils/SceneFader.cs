using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;

    public Image fadeImage; // ������ �̹���
    public float fadeSpeed = 1.0f; // ���̵� �ӵ� (�������� ������)
    public AudioClip clickSound; // Ŭ�� �Ҹ�

    private AudioSource audioSource;
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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0) // ù �������� �Է� ���
        {
            StartCoroutine(WaitForInput());
        }
    }

    private IEnumerator WaitForInput()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began));

        PlayClickSound();
        LoadingSceneManager.LoadScene(1);
        //StartCoroutine(FadeOutAndLoad(1)); // �� �ε��� 1�� ��ȯ
    }

    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public IEnumerator FadeOutAndLoad(int sceneNum)
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
        SceneManager.LoadScene(sceneNum);

        // �� �ε� �� ��ٸ���
        yield return new WaitForSeconds(3f); // ª�� ��� �ð����� �� �ε� ���

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
