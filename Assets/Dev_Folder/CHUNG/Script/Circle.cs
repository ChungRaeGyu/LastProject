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

    }

    IEnumerator FadeInCoroutine()
    {
        for(int i=0; i < renderers.Count; i++)
        {
            Debug.Log("renderers[i].color.a : " + renderers[i].color.a);
            while (renderers[i].color.a < 1)
            {
                Color color = new Color(0,0, 0, 0.05f);
                renderers[i].color += color;
                yield return new WaitForSecondsRealtime(0.01f);
            }
            
        }
        yield return null;
    }
}
