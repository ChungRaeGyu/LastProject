using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckControl : MonoBehaviour
{
    //��ġ : DeckManager->Canvas->DeckPanel
    //�� �� ������ GetComponent�� Find�� ���� �ʾƾ���
    
    List<CardBasic> cardObj = new List<CardBasic>(); //Queue�� �޾Ƽ� �ӽ� ������ ���� ���̴�.
    [SerializeField]GameObject prefab;
    [SerializeField] GameObject Canvas;
    [SerializeField] Text deckCount;

    #region �κ�
    //Book to Deck


    private void Start()
    {
        SetDeck();
    }

    //DeckVisualization
    private void SetDeck(){
        //�� ���δ� �ٽ� ���� 
        
        for (int i = 0; i < DataManager.Instance.LobbyDeck.Count; i++)
        {
            GameObject obj = Instantiate(DataManager.Instance.LobbyDeck[i].deckCardImage, Canvas.transform);
            obj.GetComponent<DeckListObj>().cardBasic = DataManager.Instance.LobbyDeck[i];
            //�̷��� �Ѵٰ� Obj�� CardBasic�� ���� �ʴ´�.
            obj.SetActive(true);
        }
    }

    //�巡�� �� ������� �ֱ�
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
