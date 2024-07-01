using UnityEngine;
using TMPro;

public class CardData : MonoBehaviour
{
    public CardSO cardSO;
    public TMP_Text nameLabel;
    public TMP_Text descriptionLabel;
    public TMP_Text costLabel;
    public TMP_Text abilityLabel;

    void Start()
    {
        if (cardSO != null)
        {
            UpdateUI();
        }
        //GetComponent<CardDrag>().cardSO = cardSO;
    }

    void UpdateUI()
    {
        nameLabel.text = cardSO.cardName;
        descriptionLabel.text = cardSO.description;
        costLabel.text = cardSO.cost.ToString();
        abilityLabel.text = cardSO.ability.ToString();
    }
}
