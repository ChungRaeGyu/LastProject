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
    public void AddCard(CardBasic cardObj){
        //����â���� ���߰� ��ư�� ������ ��
        //�巡���ؼ� ���� �꿴���� ȣ��
        AddObj(cardObj);
        Debug.Log("ī���߰�");
    }
    public void RemoveCard(){
        if(DataManager.Instance.deckList.Count==0)return;
        int endCard = DataManager.Instance.deckList.Count-1;
        DataManager.Instance.deckList[endCard].GetComponent<Card>().cardObj.currentCount++;
        DataManager.Instance.deckList.RemoveAt(endCard);
        LobbyManager.instance.InvokeCount();
        //UpdateDeck();
    }

    private void Start()
    {
        SetDeck();
    }

    //DeckVisualization
    private void SetDeck(){
        //�� ���δ� �ٽ� ���� 
        
        for (int i = 0; i < DataManager.Instance.deckList.Count; i++)
        {
            GameObject obj = Instantiate(prefab, Canvas.transform);
            obj.GetComponent<DeckListObj>().cardBasic = DataManager.Instance.deckList[i];
            //�̷��� �Ѵٰ� Obj�� CardBasic�� ���� �ʴ´�.
            obj.SetActive(true);
        }
    }

    public void AddObj(CardBasic cardBasic)
    {
        DataManager.Instance.deckList.Add(cardBasic);
        GameObject obj = Instantiate(prefab, Canvas.transform);
        obj.GetComponent<DeckListObj>().cardBasic= cardBasic;
        obj.SetActive(true); 
    }
    /*
    private void ClearDeck(){
        Debug.Log("Deck ClearDeck()");
        foreach (Card tempcard in cardObj)
        {
            ObjectPool.cardsObj.Enqueue(tempcard.gameObject);
            tempcard.gameObject.SetActive(false);
        }
        cardObj.Clear();
    }
    */
    #endregion
}
