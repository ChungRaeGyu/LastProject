using UnityEngine;
using UnityEngine.SceneManagement;

public class CardDrag : MonoBehaviour
{
    private Vector3 offset; // 드래그 시 마우스와 카드 사이의 거리
    private bool isDragging = false; // 드래그 중인지 확인하는 변수
    private Vector3 originalPosition; // 카드의 원래 위치
    private Quaternion originalRotation; // 카드의 원래 회전
    private Vector3 fixedPosition; // 카드의 고정 위치
    private Quaternion fixedRotation; // 카드의 고정 회전
    private CardBasic cardBasic;
    private bool isFixed = false; // 카드가 고정되었는지 여부를 확인하는 변수
    private RectTransform rectTransform; // RectTransform 변수

    public float dragLimitY = -340; // 드래그 제한 Y값, 기본적으로 -340
    private int originalSiblingIndex; // 초기 순서 저장 변수
    private CardZoom cardZoom;

    private BezierDragLine dragLine; // BezierDragLine 스크립트 참조 변수

    private void Start()
    {
        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;
        cardBasic = GetComponent<CardBasic>(); // 카드의 ScriptableObject 데이터 가져오기
        cardZoom = GetComponent<CardZoom>();

        // RectTransform 컴포넌트 가져오기
        rectTransform = GetComponent<RectTransform>();

        fixedPosition = new Vector3(0, dragLimitY, 0);
        fixedRotation = Quaternion.identity; // 고정 회전 설정

        originalSiblingIndex = transform.GetSiblingIndex();

        // BezierDragLine 스크립트 컴포넌트 가져오기
        dragLine = GetComponent<BezierDragLine>();
    }

    private void Update()
    {
        if (isDragging)
        {
            // 마우스 커서의 화면 좌표를 월드 좌표로 변환
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            transform.position = cursorWorldPoint + offset; // 마우스와의 거리 유지하며 카드 이동
            transform.SetAsLastSibling(); // 맨 위로 올리기

            // 드래그 중 카드의 anchoredPosition.y 값이 dragLimitY 이상일 때 고정 위치로 이동
            if (rectTransform.anchoredPosition.y > dragLimitY && cardBasic.dragLineCard)
            {
                isFixed = true;
                rectTransform.anchoredPosition = fixedPosition;
                transform.rotation = fixedRotation;
                cardZoom.ZoomIn();

                // 드래그 라인 그리기 시작
                dragLine.StartDrawing(transform.position);
            }
            else if (isFixed && rectTransform.anchoredPosition.y <= dragLimitY)
            {
                // 고정 상태에서 anchoredPosition.y 값이 dragLimitY 이하로 내려오면 다시 드래그 가능
                isFixed = false;
                // 드래그 라인 그리기 중지
                if (dragLine != null)
                {
                    Debug.Log("dragLine이 null이 아닙니다.");
                    dragLine.StopDrawing();
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (!GameManager.instance.handManager.setCardEnd) return;

            // 플레이어가 충분한 코스트를 가지고 있고, 플레이어의 턴일 때만 드래그 가능
            if (GameManager.instance.player != null && cardBasic != null && GameManager.instance.player.currentCost >= cardBasic.cost && GameManager.instance.playerTurn)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // 드래그 시작 시 카드의 회전을 초기화
                Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
                Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
                offset = transform.position - cursorWorldPoint; // 마우스와 카드 사이의 거리 계산
                isDragging = true; // 드래그 시작
            }
        }
        else
        {
            
        }

    }

    private void OnMouseUp()
    {
        if (dragLine != null)
        {
            dragLine.StopDrawing();
        }

        if (rectTransform.anchoredPosition.y > dragLimitY && !cardBasic.dragLineCard)
        {
            cardBasic.TryUseCard(); // 카드 사용 시도
        }
        else
        {
            cardBasic.TryUseCard();
        }

        isDragging = false;
        ResetPosition();
        transform.SetSiblingIndex(originalSiblingIndex); // 초기 순서로 되돌리기
        cardZoom.ZoomOut();
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
