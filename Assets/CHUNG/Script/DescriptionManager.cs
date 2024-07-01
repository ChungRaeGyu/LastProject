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
                _instance = new GameObject("GameManager").AddComponent<DescriptionManager>();
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
    public Card currentCard;
    public Card cardObj;

    [Header("Deck")]
    public Deck deck;

    [Header("Book")]
    public Book book;
    StringBuilder stringBuilder = new StringBuilder();

    
    public void ClosePanel()
    {
        descriptionPanel.SetActive(false);
        currentCard = null;
    }

    public void AddDeck()
    {
        currentCard.cardSO.currentCount--;
        deck.AddCard(currentCard);
    }

    public void OpenPanel()
    {
        cardObj.cardSO = currentCard.cardSO;
        stringBuilder.AppendLine($"이름 : {currentCard.cardSO.name}");
        stringBuilder.AppendLine($"직업 : {currentCard.cardSO.job}");
        stringBuilder.AppendLine($"등급 : {currentCard.cardSO.rate}");
        stringBuilder.AppendLine($"설명 : {currentCard.cardSO.description}");
        stringBuilder.AppendLine($"공격종류 : {currentCard.cardSO.kind}");
        stringBuilder.AppendLine($"능력치 : {currentCard.cardSO.ability}");

        text.text = stringBuilder.ToString();
        descriptionPanel.SetActive(true);
    }
}