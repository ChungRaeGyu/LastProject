
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;

public class CardData : MonoBehaviour
{
    new RectTransform transform;
    CardBasic cardBasic;
    Animator animator;
    Vector2 maxSize = new Vector2(5, 7.5f);
    Vector2 minSize = new Vector2(3, 4.5f);
    Coroutine coroutine;
    Image[] image;
    private void Awake()
    {
        transform = GetComponent<RectTransform>();
        cardBasic = GetComponent<CardBasic>();
        image = GetComponentsInChildren<Image>();
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


        // ��ȯ�� �� ���

    }
    public void CardOpenControl(CardBasic tempCardBasic, bool check)
    {
        image = GetComponentsInChildren<Image>();
        tempCardBasic.nameText.gameObject.SetActive(check);
        tempCardBasic.costText.gameObject.SetActive(check);
        tempCardBasic.descriptionText.gameObject.SetActive(check);
        if (check)
        {
            image[0].sprite = tempCardBasic.image;
        }
        else
        {
            image[0].sprite = DataManager.Instance.cardBackImage;
        }
    }
    private float ConvertRange(float x, float length)
    {
        float abs = Mathf.Abs(x - length/2) + length/2;

        float xNorm = length / abs; //length��ü ũ��

        return xNorm;
    }

    private void Update()
    {
        if (!LobbyManager.instance.isDrawing) return;
        float newValue = ConvertRange(transform.position.x, Camera.main.pixelWidth);

        transform.localScale = new Vector2(2.5f * newValue, 3.75f * newValue);

        if (transform.localScale.x > 4.0f)
        {
            if (coroutine == null && image[0].sprite == DataManager.Instance.cardBackImage)
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
        image[0].sprite = cardBasic.image;

        // �ؽ�Ʈ�� ���̰� �Ѵ�
        SetTextVisibility(true);

        coroutine = null;
    }
}