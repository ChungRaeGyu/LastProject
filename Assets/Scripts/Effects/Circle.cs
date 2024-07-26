using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> renderers;

    private void Start()
    {
        FadeIn();
    }
    private void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }
    private void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        for(int i=0; i < renderers.Count; i++)
        {
            while (renderers[i].color.a < 1)
            {
                Color color = new Color(0,0, 0, 0.05f);
                renderers[i].color += color;
                yield return new WaitForSecondsRealtime(0.01f);
            }
            
        }
        yield return null;
        FadeOut();
    }
    IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = renderers.Count-1; i >=0; i--)
        {
            
            while (renderers[i].color.a >0)
            {
                Color color = new Color(0, 0, 0, 0.05f);
                renderers[i].color -= color;
                yield return new WaitForSecondsRealtime(0.01f);
            }

        }
        Destroy(gameObject);

    }
}
