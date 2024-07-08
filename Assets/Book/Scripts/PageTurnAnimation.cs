using System;
using System.Collections;
using UnityEngine;

public class PageTurnAnimation : MonoBehaviour
{
    public float duration = 1f;
    public GameObject[] frames;
    public Material leftSideMaterial;
    public Material rightSideMaterial;


    public void Play(Action onComplete = null)
    {
        StartCoroutine(PlayAnimation(onComplete));
    }

    IEnumerator PlayAnimation(Action onComplete = null)
    {
        frames[0].SetActive(true);
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            int i = Mathf.RoundToInt((frames.Length - 1) * Mathf.Clamp01(t / duration));
               
            for (int j = 0; j < i; j++)
                frames[j].SetActive(false);
            frames[i].SetActive(true);

            yield return null;
        }

        foreach (GameObject frame in frames)
            frame.SetActive(false);
        onComplete?.Invoke();
    }
}
