
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
<<<<<<< Updated upstream
=======
    Image image;
>>>>>>> Stashed changes
    Animator animator;
    Vector2 maxSize = new Vector2(5, 7.5f);
    Vector2 minSize = new Vector2(3, 4.5f);
    Coroutine coroutine;
<<<<<<< Updated upstream
    Image[] image;
=======
    public bool isStartCompleted;

>>>>>>> Stashed changes
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

        // ��ȯ�� �� ���

    }
<<<<<<< Updated upstream
    public void CardOpenControl(CardBasic tempCardBasic, bool check)
    {
        image = GetComponentsInChildren<Image>();
        Debug.Log("���� : " + check);
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
=======

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
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            if (coroutine == null && image[0].sprite == DataManager.Instance.cardBackImage)
=======
            if (coroutine == null && image.sprite == DataManager.Instance.cardBackImage)
>>>>>>> Stashed changes
            {
                cardBasic.PlaySound(SettingManager.Instance.CardFlip);
                animator.SetTrigger("Flip"); // ī�带 ������
                coroutine = StartCoroutine(Delay());
            }
        }

        //1564�϶��� �������� 
    }

    // �ؽ�Ʈ���� ���̰ų� �Ⱥ��̰� �ϴ� �޼���
    public void SetTextVisibility(bool isVisible, CardBasic tempCardBasic)
    {
        tempCardBasic.nameText.gameObject.SetActive(isVisible);
        tempCardBasic.costText.gameObject.SetActive(isVisible);
        tempCardBasic.descriptionText.gameObject.SetActive(isVisible);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.2f);
<<<<<<< Updated upstream
        image[0].sprite = cardBasic.image;
=======
        image.sprite = cardBasic.image;
>>>>>>> Stashed changes

        // �ؽ�Ʈ�� ���̰� �Ѵ�
        SetTextVisibility(true, cardBasic);

        coroutine = null;
    }
}