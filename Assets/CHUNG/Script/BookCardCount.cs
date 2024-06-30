using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookCard : Card
{
    public Text text;

    private void OnEnable() {
        text.text = cardSO.currentCount.ToString();
    }
}
