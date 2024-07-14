using System;
using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CardDrag : MonoBehaviour
{
    private Vector3 offset; // �巡�� �� ���콺�� ī�� ������ �Ÿ�
    public bool isDragging = false; // �巡�� ������ Ȯ���ϴ� ����
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
    private Coroutine clickCoroutine; // Ŭ�� �ڷ�ƾ�� ������ ����
    private GameObject draggedCardPrefab;

    private void Awake()
    {
        cardBasic = GetComponent<CardBasic>();
        cardZoom = GetComponent<CardZoom>();
        // RectTransform ������Ʈ ��������
        rectTransform = GetComponent<RectTransform>();
        // BezierDragLine ��ũ��Ʈ ������Ʈ ��������
        dragLine = GetComponent<BezierDragLine>();
        originalSiblingIndex = transform.GetSiblingIndex();//Hierarchy������ ����
    }
    private void Start()
    {
        fixedPosition = new Vector3(0, dragLimitY, 0);
        fixedRotation = Quaternion.identity; // ���� ȸ�� ����  
    }
    public void Initialize(bool drag)
    {
        isDragging = drag;
    }
    private void Update()
    {
        if (isDragging)
        {
            // ���콺 Ŀ���� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            transform.position = cursorWorldPoint; // ���콺���� �Ÿ� �����ϸ� ī�� �̵�
            transform.SetAsLastSibling(); // �� ���� �ø���

            if (SceneManager.GetActiveScene().buildIndex != 3) return;

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
            clickCoroutine = StartCoroutine(OnClickDetect());
        }
    }

    private IEnumerator OnClickDetect()
    {
        float clickTime = 1f; // Ŭ�� ���� �ð� ���� (1��)
        float elapsedTime = 0f;
        bool isLongClick = false;

        while (elapsedTime < clickTime)
        {
            elapsedTime += Time.deltaTime;

            // 1�� �̻� ������ ��
            if (elapsedTime >= clickTime)
            {
                isLongClick = true;
                break;
            }

            yield return null; // �� �����Ӿ� ���
        }

        if (isLongClick)
        {
            GameObject Canvas = transform.root.GetChild(1).gameObject;
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorWorldPoint.z = 0;
            draggedCardPrefab = Instantiate(cardBasic.gameObject, cursorWorldPoint,Quaternion.identity, Canvas.transform);
            draggedCardPrefab.GetComponent<CardDrag>().Initialize(true);
            RectTransform tempRect = draggedCardPrefab.GetComponent<RectTransform>();
            tempRect.sizeDelta = new Vector2(100, 100);
            // �ش� ī�带 �����ؼ� �����ϰ� �� ������ ī�带 �巡��
            /*
            
            tempRect.localPosition = cursorWorldPoint;
            */

            // �ڷ�ƾ�� ���� �� null�� �ʱ�ȭ
            clickCoroutine = null;
        }
        else
        {
            // ���⿡ 1�� �̸����� ������ ���� ������ �����.
            Debug.Log("��¦ ������.");

            // �ڷ�ƾ�� ���� �� null�� �ʱ�ȭ
            clickCoroutine = null;
        }
    }

    private void OnMouseUp()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (dragLine != null)
            {
                dragLine.StopDrawing();
            }

            if (!cardBasic.dragLineCard)
            {
                if (rectTransform.anchoredPosition.y > dragLimitY)
                {
                    cardBasic.TryUseCard(); // ī�� ��� �õ�
                }
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
        else
        {
            //TODO : �����ϰų� ���� ��ġ������ �� �߰�
            if (LobbyManager.instance.currentCanvas == LobbyManager.instance.deckCanvas)
            {
                try
                {
                    LobbyManager.instance.deckControl.AddObj(draggedCardPrefab.GetComponent<CardBasic>().cardBasic);
                }
                catch (Exception err)
                {

                    Debug.Log("Message"+err.Message);
                }
                
            }
            Destroy(draggedCardPrefab);
        }


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
