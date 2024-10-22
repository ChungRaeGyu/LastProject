using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckControl : MonoBehaviour
{
    //위치 : DeckManager->Canvas->DeckPanel
    //될 수 있으면 GetComponent와 Find를 쓰지 않아야함
    
    List<CardBasic> cardObj = new List<CardBasic>(); //Queue를 받아서 임시 저장해 놓는 곳이다.
    [SerializeField]GameObject prefab;
    [SerializeField] GameObject Canvas;
    [SerializeField] TextMeshProUGUI deckCount;

    #region 로비
    //Book to Deck


    private void Start()
    {
        SetDeck();
    }

    //DeckVisualization
    private void SetDeck(){
        //덱 전부다 다시 생성 
        
        for (int i = 0; i < DataManager.Instance.LobbyDeck.Count; i++)
        {
            GameObject obj = Instantiate(DataManager.Instance.LobbyDeck[i].deckCardImage, Canvas.transform);
            obj.GetComponent<DeckListObj>().cardBasic = DataManager.Instance.LobbyDeck[i];
            //이렇게 한다고 Obj의 CardBasic에 들어가지 않는다.
            obj.SetActive(true);
        }
    }

    //드래그 앤 드랍으로 넣기
    public void AddCardObj(CardBasic cardBasic)
    {
        if (RateCheck(cardBasic)) return;
        cardBasic.currentCount--;
        LobbyManager.instance.InvokeCount();
        DataManager.Instance.LobbyDeck.Add(cardBasic);
        DataManager.Instance.LobbyDeckRateCheck[(int)cardBasic.rate]++;
        GameObject obj = Instantiate(cardBasic.deckCardImage, Canvas.transform);
        obj.SetActive(true);
        DeckTextUpdate();
    }
    public void RemoveCardObj(CardBasic cardBasic)
    {
        cardBasic.currentCount++;
        LobbyManager.instance.InvokeCount();
        DataManager.Instance.LobbyDeck.Remove(cardBasic);
        DeckTextUpdate();
    }

    private bool RateCheck(CardBasic cardBasic)
    {
        if (cardBasic.rate == Rate.Rarity)
        {
            if (DataManager.Instance.LobbyDeckRateCheck[(int)cardBasic.rate] >= 2)
            {
                return true;
            }
            else
                return false;
        }else if(cardBasic.rate == Rate.Hero||cardBasic.rate==Rate.Legend)
        {
            if (DataManager.Instance.LobbyDeckRateCheck[(int)cardBasic.rate] >= 1)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }
    private void DeckTextUpdate()
    {
        deckCount.text = $"{DataManager.Instance.LobbyDeck.Count.ToString()}";
        deckCount.color = DataManager.Instance.LobbyDeck.Count<10?Color.red: Color.white;
    }
    #endregion
}
