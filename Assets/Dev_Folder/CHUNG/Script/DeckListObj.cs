using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckListObj : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public CardBasic cardBasic;
    // Start is called before the first frame update
    void Start()
    {
        text.text = cardBasic.cardName;
    }
}
