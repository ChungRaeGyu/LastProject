using UnityEngine;

public class CardDrag : MonoBehaviour
{
    private Vector3 offset; // �巡�� �� ���콺�� ī�� ������ �Ÿ�
    private bool isDragging = false; // �巡�� ������ Ȯ���ϴ� ����
    private Vector3 originalPosition; // ī���� ���� ��ġ
    private Quaternion originalRotation; // ī���� ���� ȸ��
    public CardData cardObj; // ī���� ScriptableObject ������
    public Player player { get; private set; } // �÷��̾� ��ü

    private void Start()
    {
        player = GameManager.instance.player; // ���� �Ŵ������� �÷��̾� ��ü ��������
        cardObj = GetComponent<CardData>(); // ī���� ScriptableObject ������ ��������
    }

    private void Update()
    {
        if (isDragging)
        {
            // ���콺 Ŀ���� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            transform.position = cursorWorldPoint + offset; // ���콺���� �Ÿ� �����ϸ� ī�� �̵�
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
        }
    }

    private void OnMouseUp()
    {
        isDragging = false; // �巡�� ����
        GetComponent<CardBasic>().TryUseCard(); // ī�� ��� �õ�
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
