using UnityEngine;

public class CardClickTest : MonoBehaviour
{
    public GameObject cardPrefab; // Unity Inspector에서 카드 프리팹을 할당

    private HandManager handManager;

    private void Start()
    {
        handManager = FindObjectOfType<HandManager>(); // HandManager 스크립트를 찾아 할당
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 마우스 우클릭 감지
        {
            Vector3 spawnPosition = transform.position; // 카드가 생성될 위치 설정 (임시로 여기서 생성)

            GameObject newCard = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);
            handManager.AddCard(newCard.transform); // HandManager에 생성된 카드 추가
        }
    }
}
