
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
    public bool isStartCompleted;

    private void Awake()
    {
        transform = GetComponent<RectTransform>();
        cardBasic = GetComponent<CardBasic>();
        image = transform.GetChild(1).GetComponent<Image>();
        animator = GetComponent<Animator>();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            animator.enabled = true;
            this.enabled = true;
            if (!LobbyManager.instance.isDrawing)
                CardOpenControl(cardBasic, cardBasic.cardBasic.isFind);
        }
        else
        {
            animator.enabled = false;
            this.enabled = false;
        }

        // 변환된 값 계산

    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            this.enabled = true;
            if (cardBasic.cardBasic != null)
                CardOpenControl(cardBasic, cardBasic.cardBasic.isFind);
        }
        else this.enabled = false;

        isStartCompleted = true;
    }

    public void CardOpenControl(CardBasic tempCardBasic, bool check)
    {
        SetTextVisibility(check, tempCardBasic);
        if (check)
        {
            image.sprite = tempCardBasic.image;

            switch (tempCardBasic.enhancementLevel)
            {
                case 1:
                    image.sprite = tempCardBasic.firstEnhanceImage;
                    break;
                case 2:
                    image.sprite = tempCardBasic.secondEnhanceImage;
                    break;
                default:
                    break;
            }
        }
        else
        {
            image.sprite = DataManager.Instance.cardBackImage;
        }
    }

    private float ConvertRange(float x, float length)
    {
        float abs = Mathf.Abs(x - length / 2) + length / 2;

        float xNorm = length / abs; //length전체 크기

        return xNorm;
    }

    private void Update()
    {
        if (!LobbyManager.instance.isDrawing) return;
        float newValue = ConvertRange(transform.position.x, Camera.main.pixelWidth);

        transform.localScale = new Vector2(2.5f * newValue, 3.75f * newValue);

        if (transform.localScale.x > 4.0f)
        {
            if (coroutine == null && image.sprite == DataManager.Instance.cardBackImage)
            {
                cardBasic.PlaySound(SettingManager.Instance.CardFlip);
                animator.SetTrigger("Flip"); // 카드를 뒤집음
                coroutine = StartCoroutine(Delay());
            }
        }

        //1564일때를 기준으로 
    }

    // 텍스트들을 보이거나 안보이게 하는 메서드
    public void SetTextVisibility(bool isVisible, CardBasic tempCardBasic)
    {
        tempCardBasic.nameText.gameObject.SetActive(isVisible);
        tempCardBasic.costText.gameObject.SetActive(isVisible);
        tempCardBasic.descriptionText.gameObject.SetActive(isVisible);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        image.sprite = cardBasic.image;

        // 텍스트가 보이게 한다
        SetTextVisibility(true, cardBasic);

        coroutine = null;
    }
}