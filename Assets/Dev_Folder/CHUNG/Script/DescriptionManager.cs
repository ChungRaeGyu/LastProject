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
    int num=1;
    public void ClosePanel()
    {
        descriptionPanel.SetActive(false);
        currentCard = null;
    }

    public void AddDeck()
    {
        //덱추가 버튼
        currentCard.currentCount--;
        deck.AddCard(currentCard);
        LobbyManager.instance.InvokeCount();
        ClosePanel();
    }

    public void OpenPanel()
    {
        descriptionPanel.SetActive(true);
    }

    public void DeCompositionPanelBtn(){
        //분해창 오픈
        deCompositionPanel.SetActive(!deCompositionPanel.activeInHierarchy);
        num=1;
    }

    public void RightBtn(){
        if(num>=currentCard.currentCount)return;
        num++;
        cardSubject.text = num.ToString();
    }
    public void LeftBtn(){
        if(num<=1)return;
        num--;
        cardSubject.text = num.ToString();
    }
    public void DeCompositionBtn(){
        currentCard.currentCount-=num;
        DataManager.Instance.CardPiece[(int)currentCard.rate]+=num;
        if(currentCard.currentCount==0){
            DeCompositionPanelBtn();
            ClosePanel();
        }
        num=1;
    }
}