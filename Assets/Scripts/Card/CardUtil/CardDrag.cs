using System;
using System.Collections;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector3 offset; // 드래그 시 마우스와 카드 사이의 거리
    public bool isDragging = false; // 드래그 중인지 확인하는 변수
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
    private Coroutine clickCoroutine; // 클릭 코루틴을 저장할 변수
    private GameObject draggedCardPrefab;
    private bool isClick = false;
    private bool isLongClick = false;
    private void Awake()
    {
        cardBasic = GetComponent<CardBasic>();
        cardZoom = GetComponent<CardZoom>();
        // RectTransform 컴포넌트 가져오기
        rectTransform = GetComponent<RectTransform>();
        // BezierDragLine 스크립트 컴포넌트 가져오기
        dragLine = GetComponent<BezierDragLine>();
        originalSiblingIndex = transform.GetSiblingIndex();//Hierarchy에서의 순서
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            // 하위 이미지의 RaycastTarget 속성 켜기
            Image image = GetComponentInChildren<Image>();
            if (image != null)
            {
                image.raycastTarget = true;
            }
        }
    }
    private void Start()
    {
        fixedPosition = new Vector3(0, dragLimitY, 0);
        fixedRotation = Quaternion.identity; // 고정 회전 설정  
    }
    public void Initialize(bool drag)
    {
        isDragging = drag;
    }
    private void Update()
    {
        if (isDragging)
        {

            // 마우스 커서의 화면 좌표를 월드 좌표로 변환
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            transform.position = cursorWorldPoint; // 마우스와의 거리 유지하며 카드 이동
            transform.SetAsLastSibling(); // 맨 위로 올리기
            cardZoom.ZoomIn();

            if (SceneManager.GetActiveScene().buildIndex != 3) return;

            // 드래그 중 카드의 anchoredPosition.y 값이 dragLimitY 이상일 때 고정 위치로 이동
            if (rectTransform.anchoredPosition.y > dragLimitY && cardBasic.dragLineCard)
            {
                isFixed = true;
                rectTransform.anchoredPosition = fixedPosition;
                transform.rotation = fixedRotation;

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
                    dragLine.StopDrawing();
                }
            }
        }
    }

    private IEnumerator OnClickDetect()
    {
        float clickTime = 0.3f; // 클릭 감지 시간 설정 (1초)
        float elapsedTime = 0f;

        isClick = true;
        if (cardBasic.cardBasic.currentCount != 0)
        {
            while (isClick)
            {
                elapsedTime += Time.deltaTime;

                // 1초 이상 눌렀을 때
                if (elapsedTime >= clickTime)
                {
                    cardBasic.PlaySound(SettingManager.Instance.CardSelect);

                    isLongClick = true;
                    isClick = false;
                }

                yield return null; // 한 프레임씩 대기
            }
        }

        if (isLongClick)
        {
            GameObject Canvas = transform.root.GetChild(1).gameObject;
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorWorldPoint.z = 0;
            draggedCardPrefab = Instantiate(cardBasic.gameObject, cursorWorldPoint, Quaternion.identity, Canvas.transform);
            Destroy(draggedCardPrefab.transform.GetChild(2).gameObject);
            draggedCardPrefab.GetComponentInChildren<Image>().raycastTarget = false;


            draggedCardPrefab.GetComponent<CardDrag>().Initialize(true);
            RectTransform tempRect = draggedCardPrefab.GetComponent<RectTransform>();
            Debug.Log("누른 카드 : " + draggedCardPrefab.name);
            tempRect.sizeDelta = new Vector2(100, 100);
            tempRect.localScale = new Vector3(2, 3, 1);
            // 해당 카드를 복제해서 생성하고 그 복제한 카드를 드래그
            /*
            
            tempRect.localPosition = cursorWorldPoint;
            */

            // 코루틴이 끝난 후 null로 초기화
            clickCoroutine = null;
        }
        else
        {
            if (DescriptionManager.Instance.descriptionPanel.activeInHierarchy) yield break;
            if (SettingManager.Instance.SoundPanel.activeInHierarchy) yield break;
            DescriptionManager.Instance.OpenPanel(cardBasic);

            // 여기에 1초 미만으로 눌렀을 때의 로직을 만든다.
            Debug.Log("살짝 눌렀다.");

            //audioSource.PlayOneShot(AudioManager.Instance.CardPassClip);        
            // 코루틴이 끝난 후 null로 초기화
            clickCoroutine = null;
        }
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

    public void OnPointerDown(PointerEventData eventData)
    {

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (GameManager.instance.player?.IsDead() == true) return;

            if (!GameManager.instance.handManager.setCardEnd) return;

            // 플레이어가 충분한 코스트를 가지고 있고, 플레이어의 턴일 때만 드래그 가능
            if (GameManager.instance.player != null && cardBasic != null && GameManager.instance.player.currentCost >= cardBasic.cost && GameManager.instance.playerTurn)
            {
                cardBasic.PlaySound(SettingManager.Instance.CardSelect);

                transform.rotation = Quaternion.Euler(0, 0, 0); // 드래그 시작 시 카드의 회전을 초기화
                Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
                Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
                offset = transform.position - cursorWorldPoint; // 마우스와 카드 사이의 거리 계산
                isDragging = true; // 드래그 시작
            }
        }
        else if(SceneManager.GetActiveScene().buildIndex==1)
        {
            if (!cardBasic.cardBasic.isFind) return;
            if (DescriptionManager.Instance.descriptionPanel.activeInHierarchy) return;
            clickCoroutine = StartCoroutine(OnClickDetect());

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClick = false;
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (GameManager.instance.player?.IsDead() == true) return;

            if (dragLine != null)
            {
                dragLine.StopDrawing();
            }

            if (!cardBasic.dragLineCard)
            {
                if (rectTransform.anchoredPosition.y > dragLimitY)
                {
                    GameManager.instance.cardQueue.Enqueue(cardBasic);
                    transform.GetChild(0).gameObject.SetActive(false);
                    enabled = false;
                }
            }
            else
            {
                if (dragLine.detectedMonster != null)
                {
                    GameManager.instance.cardQueue.Enqueue(cardBasic);
                    transform.GetChild(0).gameObject.SetActive(false);
                    enabled = false;
                }
            }

            isDragging = false;
            cardBasic.PlaySound(SettingManager.Instance.CardDrop);
            ResetPosition();
            transform.SetSiblingIndex(originalSiblingIndex); // 초기 순서로 되돌리기
            cardZoom.ZoomOut();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (!cardBasic.cardBasic.isFind) return;
            if (!isLongClick) return;
            isLongClick = false;
            //로비에서 드래그 사용
            if (LobbyManager.instance.currentCanvas == LobbyManager.instance.deckCanvas)
            {
                cardBasic.PlaySound(SettingManager.Instance.CardDrop);

                LobbyManager.instance.deckControl.AddCardObj(draggedCardPrefab.GetComponent<CardBasic>().cardBasic);
            }
            Destroy(draggedCardPrefab);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
