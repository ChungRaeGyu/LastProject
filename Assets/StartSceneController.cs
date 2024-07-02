using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    public Image fadeImage; // ������ �̹���
    public float fadeSpeed = 1.0f; // ���̵� �ӵ� (�������� ������)
    public AudioClip clickSound; // Ŭ�� �Ҹ�

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
#if UNITY_EDITOR // ����Ƽ ������
            if (Input.GetMouseButtonDown(0))
            {
                PlayClickSound();
                SceneFader.FadeOutAndLoadScene(1, fadeImage, fadeSpeed);
            }
#elif UNITY_ANDROID || UNITY_IOS // �ȵ���̵� �Ǵ� iOS
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
