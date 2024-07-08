using UnityEngine;

public class CardDrag : MonoBehaviour
{
    private Vector3 offset; // �巡�� �� ���콺�� ī�� ������ �Ÿ�
    private bool isDragging = false; // �巡�� ������ Ȯ���ϴ� ����
    private Vector3 originalPosition; // ī���� ���� ��ġ
    private Quaternion originalRotation; // ī���� ���� ȸ��
    public CardData cardObj; // ī���� ScriptableObject ������
    private LineRenderer lineRenderer;
    private Vector3 startMousePosition, endMousePosition;
    private BezierCurve bezierCurve;
    public Player player { get; private set; } // �÷��̾� ��ü

    private void Start()
    {
        player = GameManager.instance.player; // ���� �Ŵ������� �÷��̾� ��ü ��������
        cardObj = GetComponent<CardData>(); // ī���� ScriptableObject ������ ��������
        lineRenderer = GetComponent<LineRenderer>();
        bezierCurve = GetComponent<BezierCurve>();
        lineRenderer.positionCount = 50; // ���� �� (����ȭ ������ ����)
    }

    private void Update()
    {
        if (isDragging)
        {
            // ���콺 Ŀ���� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            transform.position = cursorWorldPoint + offset; // ���콺���� �Ÿ� �����ϸ� ī�� �̵�

            if (isDragging) // transform.position�� Ư�� y���� �Ѱ��� ��
            {
                // ī���� ��ġ�� �����Ѵ�.
                endMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                bezierCurve.p3 = endMousePosition;

                // ������ ����: �ε巴�� �־������� ����
                Vector3 direction = (endMousePosition - startMousePosition).normalized;
                float distance = Vector3.Distance(startMousePosition, endMousePosition);
                Vector3 controlOffset = Vector3.up * distance / 2f; // ��� �־��� ���� ����

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
        // �÷��̾ ����� �ڽ�Ʈ�� ������ �ְ�, �÷��̾��� ���� ���� �巡�� ����
        if (player != null && cardObj != null && player.currentCost >= cardObj.CardObj.cost && GameManager.instance.playerTurn)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // �巡�� ���� �� ī���� ȸ���� �ʱ�ȭ
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            offset = transform.position - cursorWorldPoint; // ���콺�� ī�� ������ �Ÿ� ���
            isDragging = true; // �巡�� ����

            // �� ī�尡 �巡�װ� �ʿ��� ī���� ���� Ȯ��
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            bezierCurve.p0 = transform.position;
        }
    }

    private void OnMouseUp()
    {
        // �ݶ��̴��� ��°� ���� �� �巡���ؾ� �ϴ� ī��� Ŭ���ؾ� ��밡��
        //if (Input.GetMouseButtonDown(1))
        isDragging = false; // �巡�� ����
        GetComponent<CardBasic>().TryUseCard(); // ī�� ��� �õ�
        // ��� �� �ƹ��ϵ� ���� ��(Ʈ���ſ� ���� ������), �巡�� ������ �����Ǹ�, �ٽ� ��Ŭ���� ī�� ���
        // ��Ŭ�� �ϸ� ���� ��ġ�� �ǵ��ư���
        ResetPosition(); // �巡�� ���� �� ���� ��ġ�� �ǵ�����
    }

    public void SetOriginalPosition(Vector3 position, Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
    }

    public void ResetPosition()
    {
        transform.position = originalPosition; // ī�� ��ġ �ʱ�ȭ
        transform.rotation = originalRotation; // ī�� ȸ�� �ʱ�ȭ
    }
}
