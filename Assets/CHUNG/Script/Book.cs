using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Book : MonoBehaviour
{
    List<GameObject> bookCards = new List<GameObject>();
    [Header("CardPrefab")]
    public GameObject prefab;
    [Header("BoardPanel")]
    public GameObject boardPanel;
    [Header("CardSOs")]
    public List<CardSO> cardSOs;
    void Start()
    {
        for(int i=0; i<cardSOs.Count; i++){
            GameObject obj = Instantiate(prefab, boardPanel.transform);
            obj.GetComponentInChildren<Card>().cardSO = cardSOs[i];
            bookCards.Add(obj);
        }
    }
}
