using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Book : MonoBehaviour
{   
    //BookManager-> BookCanvas -> BookPanel에 달려있다.
    List<Card> cardObj = new List<Card>(); //Queue를 받아서 임시 저장해 놓는 곳이다.
    [Header("CardSOs")]
    public List<CardSO> cardSOs;


    void OnEnable(){
        for (int i = 0; i < cardSOs.Count; i++)
        {
            GameObject obj = ObjectPool.bookCardObj.Dequeue();
            Card tempCard = obj.GetComponent<Card>();
            cardObj.Add(tempCard);
            Debug.Log("Book : " + PrefabUtility.GetPrefabAssetType(obj));
            obj.transform.SetParent(transform);
            tempCard.cardSO = cardSOs[i];
            obj.SetActive(true);
        }
    }

    void OnDisable() {
        foreach(Card obj in cardObj){
            obj.gameObject.SetActive(false);
            ObjectPool.bookCardObj.Enqueue(obj.gameObject);
        }
        Debug.Log("book"+ ObjectPool.bookCardObj.Count);
        cardObj.Clear();
    }
}
