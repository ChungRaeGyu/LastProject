
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
    Image[] image;
    Animator animator;
    Vector2 maxSize = new Vector2(5, 7.5f);
    Vector2 minSize = new Vector2(3, 4.5f);
    Coroutine coroutine;

    private void Awake()
    {
        transform = GetComponent<RectTransform>();
        cardBasic = GetComponent<CardBasic>();
        image = GetComponentsInChildren<Image>();
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
        if (image[1].sprite == DataManager.Instance.cardBackImage)
        {

        }

        if (transform.localScale.x > 5)
        {
            if (coroutine == null && image[1].sprite == DataManager.Instance.cardBackImage)
            {
                cardBasic.PlaySound(SettingManager.Instance.CardFlip);
                animator.SetTrigger("Flip"); // 카드를 뒤집음
                coroutine = StartCoroutine(Delay());
            }
        }

        //1564일때를 기준으로 
    }

    // 텍스트들을 보이거나 안보이게 하는 메서드
    public void SetTextVisibility(bool isVisible)
    {
        cardBasic.nameText.gameObject.SetActive(isVisible);
        cardBasic.costText.gameObject.SetActive(isVisible);
        cardBasic.descriptionText.gameObject.SetActive(isVisible);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        image[1].sprite = cardBasic.image;

        // 텍스트가 보이게 한다
        SetTextVisibility(true);

        coroutine = null;
    }
}