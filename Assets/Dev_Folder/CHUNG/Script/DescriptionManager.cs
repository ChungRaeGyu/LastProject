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
    public BookCardControl bookCardControl;
    public Card currentCard;

    [Header("CardObject")]
    public Card cardobj;
    StringBuilder stringBuilder = new StringBuilder();

    [Header("DeComposition")]
    [SerializeField] GameObject deCompositionPanel;
    [SerializeField] TextMeshProUGUI cardSubject;
    [SerializeField] TextMeshProUGUI pieceSubject;
    int num=1;
    public void ClosePanel()
    {
        descriptionPanel.SetActive(false);
        currentCard = null;
    }

    public void AddDeck()
    {
        if(DataManager.Instance.deckList.Count>=20)return;

        currentCard.cardObj.currentCount--;
        deck.AddCard(currentCard.cardObj);
        bookCardControl.UpdateBook();
        ClosePanel();
    }

    public void OpenPanel()
    {
        cardobj.cardObj = currentCard.cardObj;
        stringBuilder.Clear();
        stringBuilder.AppendLine($"이름 : {currentCard.cardObj.cardName}");
        stringBuilder.AppendLine($"직업 : {currentCard.cardObj.job}");
        stringBuilder.AppendLine($"등급 : {currentCard.cardObj.rate}");
        stringBuilder.AppendLine($"설명 : {currentCard.cardObj.description}");
        stringBuilder.AppendLine($"능력치 : {currentCard.cardObj.ability}");

        text.text = stringBuilder.ToString();
        descriptionPanel.SetActive(true);
    }

    public void DeCompositionPanelBtn(){
        deCompositionPanel.SetActive(!deCompositionPanel.activeInHierarchy);
        num=1;
    }

    public void RightBtn(){
        if(num>=currentCard.cardObj.currentCount)return;
        num++;
        cardSubject.text = num.ToString();
    }
    public void LeftBtn(){
        if(num<=1)return;
        num--;
        cardSubject.text = num.ToString();
    }
    public void DeCompositionBtn(){
        currentCard.cardObj.currentCount-=num;
        DataManager.Instance.CardPiece[(int)currentCard.cardObj.rate]+=num;
        if(currentCard.cardObj.currentCount==0){
            DeCompositionPanelBtn();
            ClosePanel();
        }
        num=1;
    }
}