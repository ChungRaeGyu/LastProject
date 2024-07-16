using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckListObj : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public CardBasic cardBasic;


    Vector2 offset;
    private Coroutine clickCoroutine;
    bool isLongClick = false;
    private bool isClick = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("delaySetting", 0.1f);
    }

    private void delaySetting()
    {
        if (cardBasic != null)
            text.text = cardBasic.cardName;
    }
    private void OnMouseDown()
    {

        StartCoroutine(OnClickDetect());

    }
    private void OnMouseDrag()
    {
        if (isLongClick)
        {
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorWorldPoint.z = -1;
            //offset = (Vector2)transform.position - cursorWorldPoint; // ���콺�� ī�� ������ �Ÿ� ���
            transform.position = cursorWorldPoint;

        }
    }
    private void OnMouseUp()
    {
        isLongClick = false;
        isClick = false;
        if (LobbyManager.instance.currentCanvas != LobbyManager.instance.deckCanvas)
        {
            
            LobbyManager.instance.deckControl.RemoveCardObj(cardBasic);
            Destroy(gameObject);
        }
        else
        {
            //�� �ȿ� �״�� ���� �� 
            transform.SetParent(LobbyManager.instance.deckContent.transform);

        }
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
        Debug.Log("����");
    }
}
