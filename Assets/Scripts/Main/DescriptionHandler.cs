using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject descriptionPanel; // 설명 패널
    public TMP_Text descriptionText; // 설명 텍스트
    public Transform descriptionPosition; // 위치 지정

    private void Start()
    {
        HideDescription();
    }

    // 클릭할 때
    public void OnPointerDown(PointerEventData eventData)
    {
        ShowDescription();
    }

    // 클릭을 뗄 때
    public void OnPointerUp(PointerEventData eventData)
    {
        HideDescription();
    }

    // 설명 패널 보여주기
    private void ShowDescription()
    {
        descriptionPanel.SetActive(true);
        descriptionPanel.transform.position = descriptionPosition.position;
    }

    // 설명 패널 숨기기
    private void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
}
