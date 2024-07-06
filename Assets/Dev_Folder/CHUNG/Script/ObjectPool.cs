using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Start is called before the first frame update
    public static Queue<GameObject> cardsObj = new Queue<GameObject>();
    public static Queue<GameObject> bookCardObj = new Queue<GameObject>();

    [Header("prefabs")]
    public GameObject cardsPrefab;
    public GameObject bookCardsprefab;


    private void Awake() {
        Init();
    }

    private void Init(){
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = Instantiate(cardsPrefab, transform);
            obj.SetActive(false);
            cardsObj.Enqueue(obj);
        }
        for(int i=0; i< DataManager.Instance.cardObjs.Count*2;i++){
            GameObject obj = Instantiate(bookCardsprefab, transform);
            obj.SetActive(false);
            bookCardObj.Enqueue(obj);
        }
    }
}
