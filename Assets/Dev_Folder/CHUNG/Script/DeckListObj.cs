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
        Invoke("delaySetting", 0.1f);
    }

    private void delaySetting()
    {
        Debug.Log("ONENABLE");
        if (cardBasic != null)
            text.text = cardBasic.cardName;
        else
        {
            Debug.Log("null" + cardBasic);

        }
    }
}
