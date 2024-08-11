using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckListObj : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
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
    }
    void Update()
    {
        if (isLongClick)
        {
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            cursorWorldPoint.z = -1;
            //offset = (Vector2)transform.position - cursorWorldPoint; // ���콺�� ī�� ������ �Ÿ� ���
            transform.position = cursorWorldPoint;
            //transform.position = cursorWorldPoint;
            

        }
    }
    private void delaySetting()
    {
        if (cardBasic != null)
            text.text = cardBasic.cardName;
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        if (DescriptionManager.Instance.descriptionPanel.activeInHierarchy) return;
        

        StartCoroutine(OnClickDetect());
    }
    private IEnumerator OnClickDetect()
    {
        float clickTime = 1f; // Ŭ�� ���� �ð� ���� (1��)
        float elapsedTime = 0f;


        isClick = true;
        Debug.Log("OnclickDetect()");
        while (isClick)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= clickTime)
            {
                SettingManager.Instance.PlaySound(SettingManager.Instance.CardSelect);

                transform.SetParent(LobbyManager.instance.BookCanvas.transform);
                transform.GetComponent<Image>().raycastTarget = false;
                transform.SetAsLastSibling(); // �� ���� �ø���
                isLongClick = true;
                isClick = false;
                Debug.Log("While");

            }
            yield return null; // �� �����Ӿ� ���
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    { 
        Debug.Log("PointerUP");

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
                transform.GetComponent<Image>().raycastTarget = true;

            }

            SettingManager.Instance.PlaySound(SettingManager.Instance.CardDrop);
        }
        isLongClick = false;
        isClick = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
