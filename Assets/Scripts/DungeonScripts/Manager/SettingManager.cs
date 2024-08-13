using System.Collections;
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
    public AudioSource SFXAudioSource;

    [Header("RecycleSFX")]
    public AudioClip BtnClip1;
    public AudioClip BtnClip2;
    public AudioClip CardPassClip;
    public AudioClip CardDrop;
    public AudioClip CardSelect;
    public AudioClip CardFlip;

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
    [SerializeField] private GameObject settingCanvas;

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
        if (SceneManager.GetActiveScene().buildIndex == 5)
            settingCanvas.SetActive(false);
        else
            settingCanvas.SetActive(true);

        SoundPanel.gameObject.SetActive(false);
        SetBackgroundMusicForCurrentScene();
        UpdateButtonVisibility();
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
    }

    private void SetBackgroundMusicForCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        AudioClip newBGM = null;

        switch (sceneIndex)
        {
            case 1:
                newBGM = LobbyBGM;
                break;
            case 2:
                newBGM = DungeonBGM;
                break;
            case 3:
                newBGM = MainBGM;
                break;
            default:
                newBGM = null;
                break;
        }

        if (newBGM != BGMAudioSource.clip)
        {
            StartCoroutine(CrossfadeBGM(newBGM, 1.5f));
        }
    }

    private IEnumerator CrossfadeBGM(AudioClip newClip, float duration)
    {
        if (BGMAudioSource.isPlaying)
        {
            float currentTime = 0;
            float startVolume = BGMAudioSource.volume;

            // 현재 재생 중인 BGM의 볼륨 감소
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                BGMAudioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
                yield return null;
            }

            BGMAudioSource.Stop();
            BGMAudioSource.volume = startVolume; // 볼륨을 원래 값으로 리셋
        }

        // 새 BGM 재생
        BGMAudioSource.clip = newClip;
        BGMAudioSource.loop = true;
        BGMAudioSource.Play();

        // 새 BGM의 볼륨 증가
        float targetVolume = musicBGMSlider.value;
        float fadeInTime = 0;

        while (fadeInTime < duration)
        {
            fadeInTime += Time.deltaTime;
            BGMAudioSource.volume = Mathf.Lerp(0, targetVolume, fadeInTime / duration);
            yield return null;
        }

        BGMAudioSource.volume = targetVolume; // 목표 볼륨으로 설정
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
        DataManager.Instance.deckList.Clear();
        SaveManager.Instance.accessDungeon = false;
        SaveManager.Instance.isBossStage = false;
        SaveManager.Instance.isEliteStage = false;
        SceneFader.instance.LoadSceneWithFade(1);
    }

    public void DungeonReturnBtn()
    {
        UpdateButtonVisibility();
        DataManager.Instance.deckList.Clear();
        SaveManager.Instance.accessDungeon = false;
        SaveManager.Instance.isBossStage = false;
        SaveManager.Instance.isEliteStage = false;
        SceneFader.instance.LoadSceneWithFade(2);
    }

    public void UpdateButtonVisibility()
    {
        // 현재 씬 인덱스
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (SaveManager.Instance != null)
        {
            // Dungeon 씬에서 패널이 켜졌을 때만 dungeonReturnBtn 활성화
            dungeonReturnBtn.SetActive(SaveManager.Instance.accessDungeon && sceneIndex != 1);
        }

        // Lobby 씬이 아닌 경우에만 lobbyReturnBtn 활성화
        lobbyReturnBtn.SetActive(sceneIndex != 1);
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource audioSource = AudioSourcePool.Instance.GetAudioSource();
        audioSource.clip = clip;
        audioSource.Play();

        StartCoroutine(ReturnToPoolAfterPlay(audioSource, clip.length));
    }

    private IEnumerator ReturnToPoolAfterPlay(AudioSource audioSource, float duration)
    {
        yield return new WaitForSeconds(duration);
        AudioSourcePool.Instance.ReturnAudioSource(audioSource);
    }
}
