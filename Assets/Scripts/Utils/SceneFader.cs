using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;

    public Image fadeImage; // 검정색 이미지
    public float fadeSpeed = 1.0f; // 페이드 속도 (낮을수록 느리게)
    public AudioClip clickSound; // 클릭 소리

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

        if (SceneManager.GetActiveScene().buildIndex == 0) // 첫 씬에서만 입력 대기
        {
            StartCoroutine(WaitForInput());
        }
    }

    private IEnumerator WaitForInput()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began));

        PlayClickSound();
        LoadingSceneManager.LoadScene(1);
        //StartCoroutine(FadeOutAndLoad(1)); // 씬 인덱스 1로 전환
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

        // 페이드 아웃
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        // 씬 로드
        SceneManager.LoadScene(sceneNum);

        // 씬 로드 후 기다리기
        yield return new WaitForSeconds(3f); // 짧은 대기 시간으로 씬 로드 대기

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
