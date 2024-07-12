using UnityEngine;
using UnityEngine.SceneManagement;

public class CardDrag : MonoBehaviour
{
    private Vector3 offset; // �巡�� �� ���콺�� ī�� ������ �Ÿ�
    private bool isDragging = false; // �巡�� ������ Ȯ���ϴ� ����
    private Vector3 originalPosition; // ī���� ���� ��ġ
    private Quaternion originalRotation; // ī���� ���� ȸ��
    private Vector3 fixedPosition; // ī���� ���� ��ġ
    private Quaternion fixedRotation; // ī���� ���� ȸ��
    private CardBasic cardBasic;
    private bool isFixed = false; // ī�尡 �����Ǿ����� ���θ� Ȯ���ϴ� ����
    private RectTransform rectTransform; // RectTransform ����

    public float dragLimitY = -340; // �巡�� ���� Y��, �⺻������ -340
    private int originalSiblingIndex; // �ʱ� ���� ���� ����
    private CardZoom cardZoom;

    private BezierDragLine dragLine; // BezierDragLine ��ũ��Ʈ ���� ����

    private void Start()
    {
        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;
        cardBasic = GetComponent<CardBasic>(); // ī���� ScriptableObject ������ ��������
        cardZoom = GetComponent<CardZoom>();

        // RectTransform ������Ʈ ��������
        rectTransform = GetComponent<RectTransform>();

        fixedPosition = new Vector3(0, dragLimitY, 0);
        fixedRotation = Quaternion.identity; // ���� ȸ�� ����

        originalSiblingIndex = transform.GetSiblingIndex();

        // BezierDragLine ��ũ��Ʈ ������Ʈ ��������
        dragLine = GetComponent<BezierDragLine>();
    }

    private void Update()
    {
        if (isDragging)
        {
            // ���콺 Ŀ���� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            transform.position = cursorWorldPoint + offset; // ���콺���� �Ÿ� �����ϸ� ī�� �̵�
            transform.SetAsLastSibling(); // �� ���� �ø���

            // �巡�� �� ī���� anchoredPosition.y ���� dragLimitY �̻��� �� ���� ��ġ�� �̵�
            if (rectTransform.anchoredPosition.y > dragLimitY && cardBasic.dragLineCard)
            {
                isFixed = true;
                rectTransform.anchoredPosition = fixedPosition;
                transform.rotation = fixedRotation;
                cardZoom.ZoomIn();

                // �巡�� ���� �׸��� ����
                dragLine.StartDrawing(transform.position);
            }
            else if (isFixed && rectTransform.anchoredPosition.y <= dragLimitY)
            {
                // ���� ���¿��� anchoredPosition.y ���� dragLimitY ���Ϸ� �������� �ٽ� �巡�� ����
                isFixed = false;
                // �巡�� ���� �׸��� ����
                if (dragLine != null)
                {
                    Debug.Log("dragLine�� null�� �ƴմϴ�.");
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
            cardBasic.TryUseCard(); // ī�� ��� �õ�
        }
        else
        {
            cardBasic.TryUseCard();
        }

        isDragging = false;
        ResetPosition();
        transform.SetSiblingIndex(originalSiblingIndex); // �ʱ� ������ �ǵ�����
        cardZoom.ZoomOut();
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
