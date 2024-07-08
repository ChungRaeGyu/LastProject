using UnityEngine;

public class CardDrag : MonoBehaviour
{
    private Vector3 offset; // 드래그 시 마우스와 카드 사이의 거리
    private bool isDragging = false; // 드래그 중인지 확인하는 변수
    private Vector3 originalPosition; // 카드의 원래 위치
    private Quaternion originalRotation; // 카드의 원래 회전
    public CardData cardObj; // 카드의 ScriptableObject 데이터
    private LineRenderer lineRenderer;
    private Vector3 startMousePosition, endMousePosition;
    private BezierCurve bezierCurve;
    public Player player { get; private set; } // 플레이어 객체

    private void Start()
    {
        player = GameManager.instance.player; // 게임 매니저에서 플레이어 객체 가져오기
        cardObj = GetComponent<CardData>(); // 카드의 ScriptableObject 데이터 가져오기
        lineRenderer = GetComponent<LineRenderer>();
        bezierCurve = GetComponent<BezierCurve>();
        lineRenderer.positionCount = 50; // 점의 수 (세분화 정도를 높임)
    }

    private void Update()
    {
        if (isDragging)
        {
            // 마우스 커서의 화면 좌표를 월드 좌표로 변환
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            transform.position = cursorWorldPoint + offset; // 마우스와의 거리 유지하며 카드 이동

            if (isDragging) // transform.position이 특정 y값을 넘겼을 때
            {
                // 카드의 위치를 고정한다.
                endMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                bezierCurve.p3 = endMousePosition;

                // 제어점 설정: 부드럽게 휘어지도록 조정
                Vector3 direction = (endMousePosition - startMousePosition).normalized;
                float distance = Vector3.Distance(startMousePosition, endMousePosition);
                Vector3 controlOffset = Vector3.up * distance / 2f; // 곡선의 휘어짐 정도 조절

                bezierCurve.p1 = startMousePosition + direction * (distance / 3.0f) + controlOffset;
                bezierCurve.p2 = endMousePosition - direction * (distance / 3.0f) + controlOffset;

                for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    float t = i / (lineRenderer.positionCount - 1.0f);
                    lineRenderer.SetPosition(i, bezierCurve.GetPoint(t));
                }
            }
        }
    }

    private void OnMouseDown()
    {
        // 플레이어가 충분한 코스트를 가지고 있고, 플레이어의 턴일 때만 드래그 가능
        if (player != null && cardObj != null && player.currentCost >= cardObj.CardObj.cost && GameManager.instance.playerTurn)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // 드래그 시작 시 카드의 회전을 초기화
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            offset = transform.position - cursorWorldPoint; // 마우스와 카드 사이의 거리 계산
            isDragging = true; // 드래그 시작

            // 이 카드가 드래그가 필요한 카드일 때를 확인
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            bezierCurve.p0 = transform.position;
        }
    }

    private void OnMouseUp()
    {
        // 콜라이더에 닿는게 없을 때 드래그해야 하는 카드면 클릭해야 사용가능
        //if (Input.GetMouseButtonDown(1))
        isDragging = false; // 드래그 종료
        GetComponent<CardBasic>().TryUseCard(); // 카드 사용 시도
        // 사용 시 아무일도 없을 시(트리거에 적이 없을시), 드래그 라인은 유지되며, 다시 좌클릭시 카드 사용
        // 우클릭 하면 원래 위치로 되돌아가게
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
