using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public Monster[] monsters; // 몬스터 저장소
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

        // 플레이어 할당
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // 모든 몬스터 찾아서 할당
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
