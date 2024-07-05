using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartSceneController : MonoBehaviour
{
    public Image fadeImage; // 검정색 이미지
    public float fadeSpeed = 1.0f; // 페이드 속도 (낮을수록 느리게)
    public AudioClip clickSound; // 클릭 소리

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(WaitForInput());
    }

    IEnumerator WaitForInput()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began));

        PlayClickSound();
        SceneFader.FadeOutAndLoadScene(1, fadeImage, fadeSpeed);
    }

    void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
