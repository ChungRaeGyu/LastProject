using UnityEngine;
using UnityEngine.SceneManagement;

public class CardDrag : MonoBehaviour
{
    private Vector3 offset; // �巡�� �� ���콺�� ī�� ������ �Ÿ�
    private bool isDragging = false; // �巡�� ������ Ȯ���ϴ� ����
    private Vector3 originalPosition; // ī���� ���� ��ġ
    private Quaternion originalRotation; // ī���� ���� ȸ��
    private CardBasic cardBasic;
    private bool tryUseCardCalled = false; // ��� �õ��� ȣ��Ǿ����� ���θ� �����ϴ� ����

    private void Start()
    {
        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;
        cardBasic = GetComponent<CardBasic>(); // ī���� ScriptableObject ������ ��������
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
        if (!GameManager.instance.handManager.setCardEnd) return;

        // �÷��̾ ����� �ڽ�Ʈ�� ������ �ְ�, �÷��̾��� ���� ���� �巡�� ����
        if (GameManager.instance.player != null && cardBasic != null && GameManager.instance.player.currentCost >= cardBasic.cost && GameManager.instance.playerTurn)
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
        GetComponent<CardBasic>().TryUseCard(); // ī�� ��� �õ�
        
        // �� �õ��� ȣ��� ������ �Ʒ��� �����ʰ��ϱ�

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Debug.Log("���Դ�!");
            GetComponent<CardBasic>().TryUseCard(); // ī�� ��� �õ�
            isDragging = false;
            ResetPosition();
        }
    }

    //isDragging = false; // �巡�� ����
    //ResetPosition(); // �巡�� ���� �� ���� ��ġ�� �ǵ�����

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
