using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckListObj : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerMoveHandler
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
        Invoke("delaySetting", 0.1f);
    }

    private void delaySetting()
    {
        if (cardBasic != null)
            text.text = cardBasic.cardName;
    }

    private IEnumerator OnClickDetect()
    {
        float clickTime = 1f; // Ŭ�� ���� �ð� ���� (1��)
        float elapsedTime = 0f;


        isClick = true;

        while (isClick)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= clickTime)
            {
                transform.SetParent(LobbyManager.instance.BookCanvas.transform);
                isLongClick = true;
                isClick = false;
            }
            yield return null; // �� �����Ӿ� ���
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (DescriptionManager.Instance.descriptionPanel.activeInHierarchy) return;
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
                Destroy(gameObject);
            }
            else
            {
                //�� �ȿ� �״�� ���� �� 
                transform.SetParent(LobbyManager.instance.deckContent.transform);

            }
        }
        isLongClick = false;
        isClick = false;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (isLongClick)
        {
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorWorldPoint.z = -1;
            //offset = (Vector2)transform.position - cursorWorldPoint; // ���콺�� ī�� ������ �Ÿ� ���
            rectTransform.position = cursorWorldPoint;
            //transform.position = cursorWorldPoint;
        }
    }
}
