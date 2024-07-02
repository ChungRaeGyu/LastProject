using UnityEngine;

public class CardClickTest : MonoBehaviour
{
    public GameObject cardPrefab; // Unity Inspector���� ī�� �������� �Ҵ�

    private HandManager handManager;

    private void Start()
    {
        handManager = FindObjectOfType<HandManager>(); // HandManager ��ũ��Ʈ�� ã�� �Ҵ�
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ���콺 ��Ŭ�� ����
        {
            Vector3 spawnPosition = transform.position; // ī�尡 ������ ��ġ ���� (�ӽ÷� ���⼭ ����)

            GameObject newCard = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);
            handManager.AddCard(newCard.transform); // HandManager�� ������ ī�� �߰�
        }
    }
}
