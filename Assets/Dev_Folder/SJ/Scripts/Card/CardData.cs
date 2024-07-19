using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.Collections;

public class CardData : MonoBehaviour
{
    new RectTransform transform;
    CardBasic cardBasic;
    Image image;
    Animator animator;
    Vector2 maxSize = new Vector2(5, 7.5f);
    Vector2 minSize = new Vector2(3, 4.5f);
    Coroutine coroutine;
    private void Awake()
    {
        transform = GetComponent<RectTransform>();
        cardBasic = GetComponent<CardBasic>();
        image = GetComponentInChildren<Image>();
        animator = GetComponent<Animator>();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
        }

        // 변환된 값 계산

    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) this.enabled = true;
        else this.enabled = false;

    }
    private float ConvertRange(float x, int minOrig, int maxOrig, int minNew, int maxNew)
    {
        float abs = Mathf.Abs(x - 1604) + 1604;

        float xNorm = (maxOrig - minOrig) / abs;

        // 2단계: 정규화된 값을 새로운 범위로 변환

        return xNorm;
    }

    private void Update()
    {
        if (!LobbyManager.instance.isDrawing) return;
        float newValue = ConvertRange(transform.position.x, -1178, 4386, 3, 5) * 1.5f;

        transform.localScale = new Vector2(1 * newValue, 1.5f * newValue);

        if (transform.localScale.x > 5)
        {
            if (coroutine == null && image.sprite == cardBasic.defaultImage)
            {
                animator.SetTrigger("Flip");
                coroutine = StartCoroutine(Delay());

            }
        }

        //1564일때를 기준으로 
    }
    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        image.sprite = cardBasic.image;
        coroutine = null;
    }
}
