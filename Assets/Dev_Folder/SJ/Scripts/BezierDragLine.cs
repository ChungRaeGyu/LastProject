using UnityEngine;
using UnityEngine.SceneManagement;

public class BezierDragLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool isDrawingLine = false;
    private Vector3 startCardPosition; // ī���� ���� ��ġ
    private Vector3 startMousePosition; // ���콺�� ���� ��ġ
    private Vector3 endMousePosition; // ���콺�� ���� ��ġ
    private Vector3 controlPoint1, controlPoint2; // ������ � ������

    public LayerMask monsterLayer; // ���� ���̾ ������ �� �ִ� ����
    public Color defaultLineColor = Color.white; // �⺻ ���� ����
    public Color hitLineColor = Color.red; // ���Ϳ� �浹 �� ���� ����
    public GameObject aimingImagePrefab; // ���� �̹��� ������
    public GameObject aimingImageInstance { get; private set; } // ���� ���� �̹��� ������Ʈ
    public MonsterCharacter detectedMonster{ get; private set; }

    void Start()
    {
        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0; // �ʱ⿡�� ���� ���� ���� 0���� �����Ͽ� ������ ������ �ʰ� ��

        // ���� �̹��� �ν��Ͻ� ���� �� ��Ȱ��ȭ
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
            // ���콺�� ���� ��ġ�� ������
            endMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

            // ������ ����: �ε巴�� �־������� ����
            Vector3 direction = (endMousePosition - startCardPosition).normalized;
            float distance = Vector3.Distance(startCardPosition, endMousePosition);
            Vector3 controlOffset = Vector3.up * distance / 2f; // ��� �־��� ���� ����

            controlPoint1 = startCardPosition + direction * (distance / 3.0f) + controlOffset;
            controlPoint2 = endMousePosition - direction * (distance / 3.0f) + controlOffset;

            // ������ � ���� �׸���
            DrawBezierCurve(startCardPosition, endMousePosition, controlPoint1, controlPoint2);

            // ���Ϳ��� ���� �̹��� �����ֱ�
            ShowAimingImage(detectedMonster != null);
        }
    }

    void DrawBezierCurve(Vector3 startPos, Vector3 endPos, Vector3 control1, Vector3 control2)
    {
        lineRenderer.positionCount = 50; // ���� �� (����ȭ ������ ����)
        lineRenderer.startColor = defaultLineColor;
        lineRenderer.endColor = defaultLineColor;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float t = i / (lineRenderer.positionCount - 1.0f);
            Vector3 position = CalculateBezierPoint(t, startPos, endPos, control1, control2);
            lineRenderer.SetPosition(i, position);
        }

        // �巡�� �� ���������� �浹 �˻�
        RaycastHit2D hit = Physics2D.Raycast(endPos, Vector2.zero, Mathf.Infinity, monsterLayer);
        if (hit.collider != null)
        {
            // �浹�� ��ü�� �������� Ȯ���ϰ�, �´ٸ� �ش� ���� ����
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
            // �浹�� ������ �⺻ �������� ����
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
        lineRenderer.positionCount = 0; // ���� �����
        Debug.Log("������ ���������ϴ�.");
        DestroyAimingImage();
    }

    void ShowAimingImage(bool show)
    {
        if (show && detectedMonster != null)
        {
            // ���Ͱ� �����Ǿ��� �� ���� �̹��� Ȱ��ȭ
            aimingImageInstance.SetActive(true);
            aimingImageInstance.transform.position = detectedMonster.transform.position;

            // ������ ũ�⿡ ���缭 ���� �̹��� ũ�� ����
            Vector3 monsterScale = detectedMonster.transform.localScale;
            aimingImageInstance.transform.localScale = new Vector3(monsterScale.x, monsterScale.y, 1f);
        }
        else if (!show)
        {
            // ���Ͱ� �������� �ʾ��� �� ���� �̹��� ��Ȱ��ȭ
            aimingImageInstance.SetActive(false);
        }
    }

    public void DestroyAimingImage()
    {
        // ���� �̹��� ��Ȱ��ȭ
        if (aimingImageInstance != null)
        {
            aimingImageInstance.SetActive(false);
        }
    }
}
