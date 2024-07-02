using UnityEngine;

public class CardCollision : MonoBehaviour
{
    public Monster currentMonster { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            currentMonster = other.GetComponent<Monster>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            if (currentMonster == other.GetComponent<Monster>())
            {
                currentMonster = null;
            }
        }
    }
}
