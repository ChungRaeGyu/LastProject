using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;
using System.Text;

public class Book : MonoBehaviour
{   
    //BookManager-> BookCanvas -> BookPanel에 달려있다.
    List<GameObject> cardObj = new List<GameObject>(); //Queue를 받아서 임시 저장해 놓는 곳이다.

    List<CardSO> cardSOs = new List<CardSO>();
    private void Awake() {
        cardSOs = DataManager.Instance.cardSOs;
    }

    void OnEnable(){
        Debug.Log("Count : " + cardSOs.Count);
        for (int i = 0; i < cardSOs.Count; i++)
        {
            GameObject obj = ObjectPool.bookCardObj.Dequeue();
            Card tempCard = obj.GetComponentInChildren<Card>();
            cardObj.Add(obj);
            obj.transform.SetParent(transform);
            tempCard.cardSO = cardSOs[i];
            obj.SetActive(true);
        }
    }

    void OnDisable() {
        Debug.Log("cardObjCOunt : "+ cardObj.Count);
        foreach(GameObject obj in cardObj){
            ObjectPool.bookCardObj.Enqueue(obj);
            obj.gameObject.SetActive(false);
            
        }
        cardObj.Clear();
    }
}
