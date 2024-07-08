using UnityEngine;

public class BezierDragLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool isDragging = false;
    private Vector3 startMousePosition, endMousePosition;
    private BezierCurve bezierCurve;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        bezierCurve = GetComponent<BezierCurve>();
        lineRenderer.positionCount = 50; // 점의 수 (세분화 정도를 높임)
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            bezierCurve.p0 = startMousePosition;
        }

        if (isDragging)
        {
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

        if (Input.GetMouseButtonDown(1))
        {
            isDragging = false;
        }
    }
}
