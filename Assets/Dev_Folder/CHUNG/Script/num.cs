using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class num : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text;
    public CardBasic cardBasic;
    void Start()
    {
        LobbyManager.instance.OnCount += CountUpdate;
        text = GetComponentInChildren<TextMeshProUGUI>();
        cardBasic = GetComponentInParent<CardBasic>();
        text.text = cardBasic.currentCount.ToString();
    }

    public void CountUpdate()
    {
        Debug.Log("CoutUpdate½ÇÇà");
        text.text = cardBasic.currentCount.ToString();
    }
}
