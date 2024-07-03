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

        if (player == null)
        {
            Debug.Log("Player�� ����.");
        }
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

                if (GameManager.instance.AllMonstersDead())
                {
                    GameManager.instance.TurnEndButton.gameObject.SetActive(false);
                    GameManager.instance.lobbyButton.gameObject.SetActive(true);
                }

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
        if (player != null && player.animator != null)
        {
            player.animator.SetTrigger("Attack");
        }
    }
}
