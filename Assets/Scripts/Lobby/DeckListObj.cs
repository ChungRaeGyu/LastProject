using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckListObj : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] TextMeshProUGUI text;
    public CardBasic cardBasic;


    Vector2 offset;
    private Coroutine clickCoroutine;
    bool isLongClick = false;
    private bool isClick = false;
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        delaySetting();

        Debug.Log("cardBasic" + cardBasic);
    }
    void Update()
    {
        if (isLongClick)
        {
            Debug.Log("PointerMove");
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            cursorWorldPoint.z = -1;
            //offset = (Vector2)transform.position - cursorWorldPoint; // 마우스와 카드 사이의 거리 계산
            rectTransform.position = cursorWorldPoint;
            //transform.position = cursorWorldPoint;
            transform.SetAsLastSibling(); // 맨 위로 올리기

        }
    }
    private void delaySetting()
    {
        if (cardBasic != null)
            text.text = cardBasic.cardName;
    }

    private IEnumerator OnClickDetect()
    {
        float clickTime = 1f; // 클릭 감지 시간 설정 (1초)
        float elapsedTime = 0f;


        isClick = true;
        Debug.Log("OnclickDetect()");
        while (isClick)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= clickTime)
            {
                transform.SetParent(LobbyManager.instance.BookCanvas.transform);
                isLongClick = true;
                isClick = false;
                Debug.Log("While");

            }
            yield return null; // 한 프레임씩 대기
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (DescriptionManager.Instance.descriptionPanel.activeInHierarchy) return;
        Debug.Log("OnpointerDown");
        StartCoroutine(OnClickDetect());
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (DescriptionManager.Instance.descriptionPanel.activeInHierarchy) return;
        if (isLongClick)
        {
            if (LobbyManager.instance.currentCanvas != LobbyManager.instance.deckCanvas)
            {

                LobbyManager.instance.deckControl.RemoveCardObj(cardBasic);
                DataManager.Instance.LobbyDeckRateCheck[(int)cardBasic.rate]--;
                //Destroy(gameObject);
            }
            else
            {
                //덱 안에 그대로 있을 때 
                transform.SetParent(LobbyManager.instance.deckContent.transform);

            }
        }
        Debug.Log("PointerUP");
        isLongClick = false;
        isClick = false;
    }
}
