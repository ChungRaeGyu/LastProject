using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DeckControl : MonoBehaviour
{
    //��ġ : DeckManager->Canvas->DeckPanel
    //�� �� ������ GetComponent�� Find�� ���� �ʾƾ���
    
    List<CardBasic> cardObj = new List<CardBasic>(); //Queue�� �޾Ƽ� �ӽ� ������ ���� ���̴�.
    [SerializeField]GameObject prefab;
    [SerializeField] GameObject Canvas;

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
        if (DataManager.Instance.LobbyDeck.Count == 6)
        {
            Debug.Log("6���� ���� á���ϴ�.");
            return;
        }
        if (RateCheck(cardBasic)) return;
        cardBasic.currentCount--;
        LobbyManager.instance.InvokeCount();
        DataManager.Instance.LobbyDeck.Add(cardBasic);
        DataManager.Instance.LobbyDeckRateCheck[(int)cardBasic.rate]++;
        GameObject obj = Instantiate(cardBasic.deckCardImage, Canvas.transform);
        obj.GetComponent<DeckListObj>().cardBasic = cardBasic;
        obj.SetActive(true); 
    }
    public void RemoveCardObj(CardBasic cardBasic)
    {
        cardBasic.currentCount++;
        LobbyManager.instance.InvokeCount();
        DataManager.Instance.LobbyDeck.Remove(cardBasic);
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
    #endregion
}
