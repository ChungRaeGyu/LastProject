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
        else if(other.gameObject.name=="Deck")
        {
            Debug.Log("콜리더 들어옴");
            LobbyManager.instance.currentCanvas = other.GetComponent<GameObject>();
        }
        else
        {
            Debug.Log(other.gameObject.name + " 콜리더 ");

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
        else if (other.gameObject.name == "Deck")
        {
            Debug.Log("콜리더 나옴");
            LobbyManager.instance.currentCanvas = null;
        }
        else
        {
            Debug.Log(other.gameObject.name + " 콜리더 ");
        }
    }
}
