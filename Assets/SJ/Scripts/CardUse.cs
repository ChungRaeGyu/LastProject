using UnityEngine;

public class CardUse : MonoBehaviour
{
    public int damageAmount = 10;
    private CardDrag cardDrag;

    private void Start()
    {
        cardDrag = GetComponent<CardDrag>();
    }

    public void TryUseCard()
    {
        Monster targetMonster = CardCollision.currentMonster;
        if (targetMonster != null)
        {
            targetMonster.TakeDamage(damageAmount);
            cardDrag.player.stats.ResetGauge();
            Destroy(gameObject);
        }
        else
        {
            cardDrag.ResetPosition();
        }
    }
}
