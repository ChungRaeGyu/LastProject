using UnityEngine;

public class CardUse : MonoBehaviour
{
    private Player player; // Player 클래스 참조 추가
    private CardDrag cardDrag;
    private CardCollision cardCollision;
    public CardSO cardSO { get; private set; }
    private void Start()
    {
        cardCollision = GetComponent<CardCollision>();
        cardDrag = GetComponent<CardDrag>();
        cardSO = GetComponent<CardData>().cardSO;
        player = GameManager.instance.player; // Player 클래스 찾아서 할당
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
                Debug.Log("플레이어 코스트가 부족합니다.");
            }
        }
        else
        {
            cardDrag.ResetPosition();
        }
    }
}
