using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionManager : MonoBehaviour
{
    #region �̱���
    private static DescriptionManager _instance;
    public static DescriptionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("DescriptionManager").AddComponent<DescriptionManager>();
                Debug.Log("����");
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
                Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    [Header("descriptionPanel")]
    public GameObject descriptionPanel;

    [Header("Deck")]
    public DeckControl deck;

    [Header("intoScript")]
    public CardBasic currentCard;

    [Header("CardObject")]
    public CardBasic cardobj;
    StringBuilder stringBuilder = new StringBuilder();

    [Header("DeComposition")]
    [SerializeField] GameObject deCompositionPanel;
    [SerializeField] TextMeshProUGUI cardSubject;
    [SerializeField] TextMeshProUGUI pieceSubject;

    [Header("MakingCard")]
    [SerializeField] GameObject MakingCardPanel;
    [SerializeField] TextMeshProUGUI CurrentpieceSubject;


    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;

    [Header("Position")]
    [SerializeField] private GameObject targetEmptyObject;

    GameObject tempCard;
    int num = 1;
    public void ClosePanel()
    {
        audioSource.PlayOneShot(SettingManager.Instance.BtnClip2);
        descriptionPanel.SetActive(false);
        currentCard = null;
        Destroy(tempCard);
        SetClose();
    }

    public void AddDeck()
    {
        audioSource.PlayOneShot(SettingManager.Instance.BtnClip1);
        //���߰� ��ư
        if (currentCard.cardBasic.currentCount <= 0) return;
        deck.AddCardObj(currentCard.cardBasic);
        LobbyManager.instance.InvokeCount();
        ClosePanel();
    }

    public void OpenPanel(CardBasic cardBasic)
    {
        audioSource.PlayOneShot(SettingManager.Instance.CardPassClip);
        currentCard = cardBasic;
        tempCard = Instantiate(cardBasic.gameObject, descriptionPanel.transform); // ī�� ���� â�� ������ ī�� ����
        Destroy(tempCard.transform.GetChild(2).gameObject);
        RectTransform tempCardRect = tempCard.GetComponent<RectTransform>();
        tempCardRect.localScale = new Vector2(3, 4.5f);

        tempCardRect.localPosition = targetEmptyObject.transform.localPosition; // ī�� ��ġ ����

        tempCardRect.sizeDelta = new Vector2(90, 90);
        descriptionPanel.SetActive(true);
    }

    public void DeCompositionPanelBtn()
    {
        if (currentCard.cardBasic.currentCount <= 0) return;
        audioSource.PlayOneShot(SettingManager.Instance.CardPassClip);
        //����â ����
        deCompositionPanel.SetActive(!deCompositionPanel.activeInHierarchy);
        num = 1;
        NumUpdate();
    }

    public void RightBtn()
    {
        audioSource.PlayOneShot(SettingManager.Instance.BtnClip1);
        if (num >= currentCard.cardBasic.currentCount) return;
        num++;
        NumUpdate();
    }
    public void LeftBtn()
    {
        audioSource.PlayOneShot(SettingManager.Instance.BtnClip1);
        if (num <= 1) return;
        num--;
        NumUpdate();
    }

    private void NumUpdate()
    {
        cardSubject.text = num.ToString();
        pieceSubject.text = num.ToString();
    }
    public void DeCompositionBtn()
    {
        audioSource.PlayOneShot(SettingManager.Instance.BtnClip2);
        currentCard.cardBasic.currentCount -= num;
        DataManager.Instance.CardPiece[(int)currentCard.rate] += num;
        if (currentCard.cardBasic.currentCount == 0)
        {
            ClosePanel();
        }
        num = 1;
        NumUpdate();
        LobbyManager.instance.InvokeCount();
    }

    //���� ��ư
    public void MakingCard()
    {
        if (DataManager.Instance.CardPiece[(int)currentCard.rate] >= 100)
        {
            DataManager.Instance.CardPiece[(int)currentCard.rate] -= 100;
            currentCard.cardBasic.currentCount++;
            CurrentpieceSubject.text = DataManager.Instance.CardPiece[(int)currentCard.cardBasic.rate].ToString();
            LobbyManager.instance.InvokeCount();
        }
    }

    public void MakingCardPanelControl()
    {
        MakingCardPanel.SetActive(!MakingCardPanel.activeInHierarchy);
        CurrentpieceSubject.text = DataManager.Instance.CardPiece[(int)currentCard.cardBasic.rate].ToString();
    }

    private void SetClose()
    {
        deCompositionPanel.SetActive(false);
        MakingCardPanel.SetActive(false);
    }

    public void EnhanceCard()
    {
        switch (currentCard.cardBasic.enhancementLevel)
        {
            case 0:
                if (DataManager.Instance.currentCrystal < 300)
                {
                    Debug.Log("ũ����Ż�� �����մϴ�.");
                    return;
                }
                DataManager.Instance.currentCrystal -= 300;
                LobbyManager.instance.currentCrystal.text = DataManager.Instance.currentCrystal.ToString();
                currentCard.EnhanceCard();
                UpdateCardUI();
                break;
            case 1:
                if (DataManager.Instance.currentCrystal < 500)
                {
                    Debug.Log("ũ����Ż�� �����մϴ�.");
                    return;
                }
                DataManager.Instance.currentCrystal -= 500;
                LobbyManager.instance.currentCrystal.text = DataManager.Instance.currentCrystal.ToString();
                currentCard.EnhanceCard();
                UpdateCardUI();
                break;
            case 2:
                Debug.Log("�̹� �ִ� ��ȭ�� �Ϸ�� ī���Դϴ�.");
                break;
            default:
                break;
        }
    }

    private void UpdateCardUI()
    {
        // ������ ǥ�õ� ī�带 �ı�
        Destroy(tempCard);

        // ��ȭ�� ī�� ������ UI�� ���� ǥ��
        tempCard = Instantiate(currentCard.cardBasic.gameObject, descriptionPanel.transform); // ī�� ���� â�� ������ ī�� ����
        RectTransform tempCardRect = tempCard.GetComponent<RectTransform>();
        tempCardRect.localScale = new Vector2(3, 4.5f);

        tempCardRect.localPosition = targetEmptyObject.transform.localPosition; // ī�� ��ġ ����

        tempCardRect.sizeDelta = new Vector2(90, 90);

        LobbyManager.instance.ReplaceCard(currentCard);
    }
}