using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM")]
    [SerializeField] private AudioSource BGMAudioSource;
    [SerializeField] private AudioClip LobbyBGM;
    [SerializeField] private AudioClip DungeonAndMainBGM;

    [Header("SFX")]
    [SerializeField] private AudioSource SFXAudioSource;
    public AudioClip BtnClip1;
    public AudioClip BtnClip2;
    public AudioClip CardPassClip;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI")]
    [SerializeField] private GameObject SoundPanel;
    [SerializeField] private Slider musicMasterSlider;
    [SerializeField] private Slider musicBGMSlider;
    [SerializeField] private Slider musicSFXSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SetBackgroundMusicForCurrentScene(); // 씬이 활성화될 때 배경음악 설정
    }

    private void Start()
    {
        musicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicBGMSlider.onValueChanged.AddListener(SetBGMVolume);
        musicSFXSlider.onValueChanged.AddListener(SetSFXVolume);

        // 슬라이더의 초기 값 설정
        SetMasterVolume(musicMasterSlider.value);
        SetBGMVolume(musicBGMSlider.value);
        SetSFXVolume(musicSFXSlider.value);

        SoundPanel.gameObject.SetActive(false);
    }

    private void SetBackgroundMusicForCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (sceneIndex)
        {
            case 1:
                // Lobby 씬
                BGMAudioSource.clip = LobbyBGM;
                break;
            case 3:
                // Dungeon 씬
                BGMAudioSource.clip = DungeonAndMainBGM;
                break;
            default:
                // 기본 배경음악
                BGMAudioSource.clip = null;
                break;
        }

        BGMAudioSource.loop = true;

        BGMAudioSource.Play();
    }

    private void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    private void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    private void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void ShowSetting()
    {
        SFXAudioSource.PlayOneShot(CardPassClip);
        SoundPanel.gameObject.SetActive(true);
    }

    public void HideSetting()
    {
        SFXAudioSource.PlayOneShot(BtnClip2);
        SoundPanel.gameObject.SetActive(false);
    }
}
