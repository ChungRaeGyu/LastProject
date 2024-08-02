
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

        // ��ȯ�� �� ���

    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            this.enabled = true;
            CardOpenControl(cardBasic, cardBasic.cardBasic.isFind);
        }
        else this.enabled = false;

    }
    public void CardOpenControl(CardBasic tempCardBasic, bool check)
    {
        tempCardBasic.nameText.gameObject.SetActive(check);
        tempCardBasic.costText.gameObject.SetActive(check);
        tempCardBasic.descriptionText.gameObject.SetActive(check);
        if (check)
        {
            image[1].sprite = cardBasic.image;
        }
        else
        {
            image[1].sprite = DataManager.Instance.cardBackImage;
        }
    }
    private float ConvertRange(float x, float length)
    {
        float abs = Mathf.Abs(x - 1000) + 1000;

        float xNorm = length / abs; //maxOrig-minOrig�� ��üũ���

        return xNorm;
    }

    private void Update()
    {
        if (!LobbyManager.instance.isDrawing) return;
        float newValue = ConvertRange(transform.position.x, 2000);

        transform.localScale = new Vector2(2.5f * newValue, 3.75f * newValue);

        if (transform.localScale.x > 4.9f)
        {
            if (coroutine == null && image[1].sprite == DataManager.Instance.cardBackImage)
            {
                cardBasic.PlaySound(SettingManager.Instance.CardFlip);
                animator.SetTrigger("Flip"); // ī�带 ������
                coroutine = StartCoroutine(Delay());
            }
        }

        //1564�϶��� �������� 
    }

    // �ؽ�Ʈ���� ���̰ų� �Ⱥ��̰� �ϴ� �޼���
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

        // �ؽ�Ʈ�� ���̰� �Ѵ�
        SetTextVisibility(true);

        coroutine = null;
    }
}