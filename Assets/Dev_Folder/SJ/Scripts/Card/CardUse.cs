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
                player.UseCost(cardSO.cost);
                targetMonster.TakeDamage(cardSO.ability);
                Destroy(gameObject);

                PlayPlayerAttackAnimation();
            }
        }
        else
        {
            cardDrag.ResetPosition();
        }
    }

    private void PlayPlayerAttackAnimation()
    {
        // 여기에 플레이어의 공격 애니메이션을 재생하는 코드를 추가합니다.
        if (player != null && player.animator != null)
        {
            player.animator.SetTrigger("Attack"); // "Attack"는 플레이어 애니메이션 컨트롤러에 있는 트리거 이름입니다.
        }
    }
}
