using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public BookCardControl bookCardControl;
    public Card currentCard;

    [Header("CardObject")]
    public Card cardobj;
    StringBuilder stringBuilder = new StringBuilder();

    [Header("DeComposition")]
    [SerializeField] GameObject deCompositionPanel;
    [SerializeField] Text cardSubject;
    [SerializeField] Text pieceSubject;
    int num=1;
    public void ClosePanel()
    {
        descriptionPanel.SetActive(false);
        currentCard = null;
    }

    public void AddDeck()
    {
        if(DataManager.Instance.deckList.Count>=20)return;
        currentCard.cardSO.currentCount--;
        deck.AddCard(currentCard);
        bookCardControl.UpdateBook();
        ClosePanel();
    }

    public void OpenPanel()
    {
        cardobj.cardSO = currentCard.cardSO;
        stringBuilder.Clear();
        stringBuilder.AppendLine($"이름 : {currentCard.cardSO.cardName}");
        stringBuilder.AppendLine($"직업 : {currentCard.cardSO.job}");
        stringBuilder.AppendLine($"등급 : {currentCard.cardSO.rate}");
        stringBuilder.AppendLine($"설명 : {currentCard.cardSO.description}");
        stringBuilder.AppendLine($"공격종류 : {currentCard.cardSO.kind}");
        stringBuilder.AppendLine($"능력치 : {currentCard.cardSO.ability}");

        text.text = stringBuilder.ToString();
        descriptionPanel.SetActive(true);
    }

    public void DeCompositionPanelBtn(){
        deCompositionPanel.SetActive(!deCompositionPanel.activeInHierarchy);
        num=1;
    }

    public void RightBtn(){
        if(num>=currentCard.cardSO.currentCount)return;
        num++;
        cardSubject.text = num.ToString();
    }
    public void LeftBtn(){
        if(num<=1)return;
        num--;
        cardSubject.text = num.ToString();
    }
    public void DeCompositionBtn(){
        currentCard.cardSO.currentCount-=num;
        DataManager.Instance.CardPiece[(int)currentCard.cardSO.rate]+=num;
        if(currentCard.cardSO.currentCount==0){
            DeCompositionPanelBtn();
            ClosePanel();
        }
        num=1;
    }
}