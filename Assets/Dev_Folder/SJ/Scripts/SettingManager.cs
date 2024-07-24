using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    [Header("BGM")]
    [SerializeField] private AudioSource BGMAudioSource;
    [SerializeField] private AudioClip LobbyBGM;
    [SerializeField] private AudioClip DungeonBGM;
    [SerializeField] private AudioClip MainBGM;

    [Header("SFX")]
    [SerializeField] private AudioSource SFXAudioSource;

    [Header("RecycleSFX")]
    public AudioClip BtnClip1;
    public AudioClip BtnClip2;
    public AudioClip CardPassClip;
    public AudioClip CardDrop;
    public AudioClip CardSelect;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("SoundUI")]
    public GameObject SoundPanel;
    [SerializeField] private Slider musicMasterSlider;
    [SerializeField] private Slider musicBGMSlider;
    [SerializeField] private Slider musicSFXSlider;

    [Header("UI")]
    [SerializeField] private GameObject lobbyReturnBtn;
    [SerializeField] private GameObject dungeonReturnBtn;

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

        lobbyReturnBtn.SetActive(false);
        dungeonReturnBtn.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SoundPanel.gameObject.SetActive(false);
        SetBackgroundMusicForCurrentScene();
        UpdateButtonVisibility();
    }

    private void Start()
    {
        musicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicBGMSlider.onValueChanged.AddListener(SetBGMVolume);
        musicSFXSlider.onValueChanged.AddListener(SetSFXVolume);

        // �����̴��� �ʱ� �� ����
        SetMasterVolume(musicMasterSlider.value);
        SetBGMVolume(musicBGMSlider.value);
        SetSFXVolume(musicSFXSlider.value);
    }

    private void SetBackgroundMusicForCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (sceneIndex)
        {
            case 1:
                // Lobby ��
                BGMAudioSource.clip = LobbyBGM;
                break;
            case 2:
                // Dungeon ��
                BGMAudioSource.clip = DungeonBGM;
                break;
            case 3:
                // main ��
                BGMAudioSource.clip = MainBGM;
                break;
            default:
                // �⺻ �������
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
        if (SoundPanel.activeSelf)
        {
            HideSetting();
        }
        else
        {
            SFXAudioSource.PlayOneShot(CardPassClip);
            SoundPanel.SetActive(true);
        }
    }

    public void HideSetting()
    {
        SFXAudioSource.PlayOneShot(BtnClip2);
        SoundPanel.gameObject.SetActive(false);
    }

    public void LobbyReturnBtn()
    {
        UpdateButtonVisibility();
        SaveManager.Instance.accessDungeon = false;
        SceneManager.LoadScene(1);
    }

    public void DungeonReturnBtn()
    {
        UpdateButtonVisibility();
        SaveManager.Instance.accessDungeon = false;
        SceneManager.LoadScene(2);
    }

    public void UpdateButtonVisibility()
    {
        // ���� �� �ε���
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (SaveManager.Instance != null)
        {
            // Dungeon ������ �г��� ������ ���� dungeonReturnBtn Ȱ��ȭ
            dungeonReturnBtn.SetActive(SaveManager.Instance.accessDungeon && sceneIndex != 1);
        }

        // Lobby ���� �ƴ� ��쿡�� lobbyReturnBtn Ȱ��ȭ
        lobbyReturnBtn.SetActive(sceneIndex != 1);
    }
}
