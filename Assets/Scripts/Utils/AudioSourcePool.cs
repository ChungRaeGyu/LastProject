using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourcePool : MonoBehaviour
{
    public static AudioSourcePool Instance { get; private set; }

    [SerializeField] private int poolSize = 10;
    [SerializeField] private AudioSource audioSourcePrefab;

    public Queue<AudioSource> availableAudioSources;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        availableAudioSources = new Queue<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newAudioSource = Instantiate(audioSourcePrefab, transform);
            newAudioSource.gameObject.SetActive(false);
            availableAudioSources.Enqueue(newAudioSource);
        }
    }

    public AudioSource GetAudioSource()
    {
        if (availableAudioSources.Count > 0)
        {
            AudioSource audioSource = availableAudioSources.Dequeue();
            audioSource.gameObject.SetActive(true);
            return audioSource;
        }
        else
        {
            AudioSource newAudioSource = Instantiate(audioSourcePrefab, transform);
            return newAudioSource;
        }
    }

    public void ReturnAudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.gameObject.SetActive(false);
        availableAudioSources.Enqueue(audioSource);
    }
}
