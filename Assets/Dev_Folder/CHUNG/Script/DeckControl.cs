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
        
        for (int i = 0; i < DataManager.Instance.deckList.Count; i++)
        {
            GameObject obj = Instantiate(prefab, Canvas.transform);
            obj.GetComponent<DeckListObj>().cardBasic = DataManager.Instance.deckList[i];
            //�̷��� �Ѵٰ� Obj�� CardBasic�� ���� �ʴ´�.
            obj.SetActive(true);
        }
    }

    //�巡�� �� ������� �ֱ�
    public void AddCardObj(CardBasic cardBasic)
    {
        cardBasic.currentCount--;
        LobbyManager.instance.InvokeCount();
        DataManager.Instance.deckList.Add(cardBasic);
        GameObject obj = Instantiate(prefab, Canvas.transform);
        obj.GetComponent<DeckListObj>().cardBasic= cardBasic;
        obj.SetActive(true); 
    }
    public void RemoveCardObj(CardBasic cardBasic)
    {
        cardBasic.currentCount++;
        LobbyManager.instance.InvokeCount();
        DataManager.Instance.deckList.Remove(cardBasic);
    }
    #endregion
}
