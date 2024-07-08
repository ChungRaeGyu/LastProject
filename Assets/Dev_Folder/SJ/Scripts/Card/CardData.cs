using UnityEngine;
using TMPro;

public class CardData : MonoBehaviour
{
    public CardBasic CardObj;
    public TMP_Text nameLabel;
    public TMP_Text descriptionLabel;
    public TMP_Text costLabel;

    void Start()
    {
        if (CardObj != null)
        {
            UpdateUI();
        }
        //GetComponent<CardDrag>().cardSO = cardSO;
    }

    void UpdateUI()
    {
        nameLabel.text = CardObj.cardName;
        descriptionLabel.text = CardObj.description;
        costLabel.text = CardObj.cost.ToString();
    }
}
