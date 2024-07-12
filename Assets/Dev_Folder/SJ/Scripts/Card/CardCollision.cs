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
            Debug.Log("�ݸ��� ����");
            LobbyManager.instance.currentCanvas = other.GetComponent<GameObject>();
        }
        else
        {
            Debug.Log(other.gameObject.name + " �ݸ��� ");

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
            Debug.Log("�ݸ��� ����");
            LobbyManager.instance.currentCanvas = null;
        }
        else
        {
            Debug.Log(other.gameObject.name + " �ݸ��� ");
        }
    }
}
