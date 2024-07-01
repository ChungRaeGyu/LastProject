using UnityEngine;

public class CardUse : MonoBehaviour
{
    private Player player; // Player Ŭ���� ���� �߰�
    private CardDrag cardDrag;
    private CardCollision cardCollision;
    public CardSO cardSO { get; private set; }
    private void Start()
    {
        cardCollision = GetComponent<CardCollision>();
        cardDrag = GetComponent<CardDrag>();
        cardSO = GetComponent<CardData>().cardSO;
        player = GameManager.instance.player; // Player Ŭ���� ã�Ƽ� �Ҵ�
    }

    public void TryUseCard()
    {
        Monster targetMonster = cardCollision.currentMonster;
        if (targetMonster != null && player != null)
        {
            if (player.currentCost >= cardSO.cost)
            {
                targetMonster.TakeDamage(cardSO.ability);
                player.UseCost(cardSO.cost);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("�÷��̾� �ڽ�Ʈ�� �����մϴ�.");
            }
        }
        else
        {
            cardDrag.ResetPosition();
        }
    }
}
