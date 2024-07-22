using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionManager : MonoBehaviour
{
    #region 싱글톤
    private static DescriptionManager _instance;
    public static DescriptionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("DescriptionManager").AddComponent<DescriptionManager>();
                Debug.Log("실행");
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
    public Text text;

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

    GameObject tempCard;
    int num = 1;
    public void ClosePanel()
    {
        descriptionPanel.SetActive(false);
        currentCard = null;
        Destroy(tempCard);
    }

    public void AddDeck()
    {
        //덱추가 버튼
        if (currentCard.cardBasic.currentCount <= 0) return;
        deck.AddCardObj(currentCard.cardBasic);
        LobbyManager.instance.InvokeCount();
        ClosePanel();
    }
    public GameObject targetEmptyObject;
    public void OpenPanel(CardBasic cardBasic)
    {
        currentCard = cardBasic;
        tempCard = Instantiate(cardBasic.gameObject, descriptionPanel.transform); // 카드 정보 창의 보여줄 카드 생성
        Destroy(tempCard.transform.GetChild(2).gameObject);
        RectTransform tempCardRect = tempCard.GetComponent<RectTransform>();
        tempCardRect.localScale = new Vector2(3, 4.5f);

        tempCardRect.localPosition = targetEmptyObject.transform.localPosition; // 카드 위치 지정

        tempCardRect.sizeDelta = new Vector2(90, 90);
        descriptionPanel.SetActive(true);
    }

    public void DeCompositionPanelBtn()
    {
        //분해창 오픈
        deCompositionPanel.SetActive(!deCompositionPanel.activeInHierarchy);
        num = 1;
        cardSubject.text = num.ToString();
    }

    public void RightBtn()
    {
        if (num >= currentCard.cardBasic.currentCount) return;
        num++;
        cardSubject.text = num.ToString();
    }
    public void LeftBtn()
    {
        if (num <= 1) return;
        num--;
        cardSubject.text = num.ToString();
    }
    public void DeCompositionBtn()
    {
        currentCard.cardBasic.currentCount -= num;
        DataManager.Instance.CardPiece[(int)currentCard.rate] += num;
        if (currentCard.cardBasic.currentCount == 0)
        {
            DeCompositionPanelBtn();
            ClosePanel();
        }
        num = 1;
        cardSubject.text = num.ToString();
        LobbyManager.instance.InvokeCount();
    }
}