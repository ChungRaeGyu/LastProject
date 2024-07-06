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

    List<CardBasic> cardBasicObj = new List<CardBasic>();
    private void Awake() {
        cardBasicObj = DataManager.Instance.cardObjs;
    }

    void OnEnable(){
        Debug.Log("Count : " + cardBasicObj.Count);
        for (int i = 0; i < cardBasicObj.Count; i++)
        {
            GameObject obj = ObjectPool.bookCardObj.Dequeue();
            Card tempCard = obj.GetComponentInChildren<Card>();
            cardObj.Add(obj);
            obj.transform.SetParent(transform);
            tempCard.cardObj = cardBasicObj[i];
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
