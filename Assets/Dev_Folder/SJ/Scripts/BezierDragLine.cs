using UnityEngine;
using UnityEngine.SceneManagement;

public class BezierDragLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool isDrawingLine = false;
    private Vector3 startCardPosition; // 카드의 시작 위치
    private Vector3 startMousePosition; // 마우스의 시작 위치
    private Vector3 endMousePosition; // 마우스의 현재 위치
    private Vector3 controlPoint1, controlPoint2; // 베지어 곡선 제어점

    public LayerMask monsterLayer; // 몬스터 레이어를 설정할 수 있는 변수
    public Color defaultLineColor = Color.white; // 기본 라인 색상
    public Color hitLineColor = Color.red; // 몬스터와 충돌 시 라인 색상
    public GameObject aimingImagePrefab; // 조준 이미지 프리팹
    public GameObject aimingImageInstance { get; private set; } // 현재 조준 이미지 오브젝트
    public MonsterCharacter detectedMonster{ get; private set; }

    void Start()
    {
        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0; // 초기에는 라인 점의 수를 0으로 설정하여 라인을 보이지 않게 함

        // 조준 이미지 인스턴스 생성 후 비활성화
        if (aimingImagePrefab != null)
        {
            aimingImageInstance = Instantiate(aimingImagePrefab);
            aimingImageInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (isDrawingLine)
        {
            // 마우스의 현재 위치를 가져옴
            endMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

            // 제어점 설정: 부드럽게 휘어지도록 조정
            Vector3 direction = (endMousePosition - startCardPosition).normalized;
            float distance = Vector3.Distance(startCardPosition, endMousePosition);
            Vector3 controlOffset = Vector3.up * distance / 2f; // 곡선의 휘어짐 정도 조절

            controlPoint1 = startCardPosition + direction * (distance / 3.0f) + controlOffset;
            controlPoint2 = endMousePosition - direction * (distance / 3.0f) + controlOffset;

            // 베지어 곡선 라인 그리기
            DrawBezierCurve(startCardPosition, endMousePosition, controlPoint1, controlPoint2);

            // 몬스터에게 조준 이미지 보여주기
            ShowAimingImage(detectedMonster != null);
        }
    }

    void DrawBezierCurve(Vector3 startPos, Vector3 endPos, Vector3 control1, Vector3 control2)
    {
        lineRenderer.positionCount = 50; // 점의 수 (세분화 정도를 높임)
        lineRenderer.startColor = defaultLineColor;
        lineRenderer.endColor = defaultLineColor;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float t = i / (lineRenderer.positionCount - 1.0f);
            Vector3 position = CalculateBezierPoint(t, startPos, endPos, control1, control2);
            lineRenderer.SetPosition(i, position);
        }

        // 드래그 중 지속적으로 충돌 검사
        RaycastHit2D hit = Physics2D.Raycast(endPos, Vector2.zero, Mathf.Infinity, monsterLayer);
        if (hit.collider != null)
        {
            // 충돌한 객체가 몬스터인지 확인하고, 맞다면 해당 몬스터 저장
            MonsterCharacter monster = hit.collider.GetComponent<MonsterCharacter>();
            if (monster != null)
            {
                detectedMonster = monster;
                lineRenderer.startColor = hitLineColor;
                lineRenderer.endColor = hitLineColor;
            }
            else
            {
                detectedMonster = null;
                lineRenderer.startColor = defaultLineColor;
                lineRenderer.endColor = defaultLineColor;
            }
        }
        else
        {
            detectedMonster = null;
            // 충돌이 없으면 기본 색상으로 설정
            lineRenderer.startColor = defaultLineColor;
            lineRenderer.endColor = defaultLineColor;
        }
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p3, Vector3 control1, Vector3 control2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * control1;
        p += 3 * u * tt * control2;
        p += ttt * p3;

        return p;
    }

    public void StartDrawing(Vector3 cardPosition)
    {
        isDrawingLine = true;
        startCardPosition = cardPosition;
        startMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }

    public void StopDrawing()
    {
        isDrawingLine = false;
        lineRenderer.positionCount = 0; // 라인 지우기
        Debug.Log("라인이 지워졌습니다.");
        DestroyAimingImage();
    }

    void ShowAimingImage(bool show)
    {
        if (show && detectedMonster != null)
        {
            // 몬스터가 감지되었을 때 조준 이미지 활성화
            aimingImageInstance.SetActive(true);
            aimingImageInstance.transform.position = detectedMonster.transform.position;

            // 몬스터의 크기에 맞춰서 조준 이미지 크기 조정
            Vector3 monsterScale = detectedMonster.transform.localScale;
            aimingImageInstance.transform.localScale = new Vector3(monsterScale.x, monsterScale.y, 1f);
        }
        else if (!show)
        {
            // 몬스터가 감지되지 않았을 때 조준 이미지 비활성화
            aimingImageInstance.SetActive(false);
        }
    }

    public void DestroyAimingImage()
    {
        // 조준 이미지 비활성화
        if (aimingImageInstance != null)
        {
            aimingImageInstance.SetActive(false);
        }
    }
}
