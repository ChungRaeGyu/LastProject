using UnityEngine;

public class CardDrag : MonoBehaviour
{
    private Vector3 offset; // 드래그 시 마우스와 카드 사이의 거리
    private bool isDragging = false; // 드래그 중인지 확인하는 변수
    private Vector3 originalPosition; // 카드의 원래 위치
    private Quaternion originalRotation; // 카드의 원래 회전
    private CardData cardSO; // 카드의 ScriptableObject 데이터
    public Player player { get; private set; } // 플레이어 객체

    private void Start()
    {
        player = GameManager.instance.player; // 게임 매니저에서 플레이어 객체 가져오기
        cardSO = GetComponent<CardData>(); // 카드의 ScriptableObject 데이터 가져오기
    }

    private void Update()
    {
        if (isDragging)
        {
            // 마우스 커서의 화면 좌표를 월드 좌표로 변환
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            transform.position = cursorWorldPoint + offset; // 마우스와의 거리 유지하며 카드 이동
        }
    }

    private void OnMouseDown()
    {
        // 플레이어가 충분한 코스트를 가지고 있고, 플레이어의 턴일 때만 드래그 가능
        if (player != null && cardSO != null && player.currentCost >= cardSO.CardObj.cost && GameManager.instance.playerTurn)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // 드래그 시작 시 카드의 회전을 초기화
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            offset = transform.position - cursorWorldPoint; // 마우스와 카드 사이의 거리 계산
            isDragging = true; // 드래그 시작
        }
    }

    private void OnMouseUp()
    {
        isDragging = false; // 드래그 종료
        GetComponent<CardBasic>().TryUseCard(); // 카드 사용 시도
        ResetPosition(); // 드래그 종료 후 원래 위치로 되돌리기
    }

    public void SetOriginalPosition(Vector3 position, Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
    }

    public void ResetPosition()
    {
        transform.position = originalPosition; // 카드 위치 초기화
        transform.rotation = originalRotation; // 카드 회전 초기화
    }
}
