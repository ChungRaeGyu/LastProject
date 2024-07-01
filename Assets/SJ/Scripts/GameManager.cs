using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public Monster[] monsters; // ���� �����
    public bool turnEnd { get; private set; } = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // �÷��̾� �Ҵ�
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // ��� ���� ã�Ƽ� �Ҵ�
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
        monsters = new Monster[monsterObjects.Length];
        for (int i = 0; i < monsterObjects.Length; i++)
        {
            monsters[i] = monsterObjects[i].GetComponent<Monster>();
        }
    }

    public void TurnEndToggle()
    {
        turnEnd = !turnEnd;
    }

    private void Update()
    {
        if (!turnEnd)
        {
            
        }
    }
}
