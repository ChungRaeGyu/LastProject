using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
#if UNITY_EDITOR // 유니티 에디터
            if (Input.GetMouseButtonDown(0))
            {
                PlayClickSound();
                SceneFader.FadeOutAndLoadScene(1, fadeImage, fadeSpeed);
            }
#elif UNITY_ANDROID || UNITY_IOS // 안드로이드 또는 iOS
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlayClickSound();
                SceneFader.FadeOutAndLoadScene("LobbyScene", fadeImage, fadeSpeed);
            }
#endif
        }
    }

    void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
