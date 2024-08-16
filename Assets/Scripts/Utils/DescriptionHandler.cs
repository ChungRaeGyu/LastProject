using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DescriptionHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject descriptionPanel; // 설명 패널
    public TMP_Text descriptionText; // 설명 텍스트
    public Transform descriptionPosition; // 위치 지정
    private Image imageComponent;

    private void Start()
    {
        HideDescription();

        imageComponent = GetComponent<Image>();
    }

    private void Update()
    {
        if (!SettingManager.Instance.SoundPanel.activeSelf && !SettingManager.Instance.ReCheckPanel.activeSelf)
        {
            if (imageComponent != null)
            {
                imageComponent.raycastTarget = true;
            }
            return;
        }
    }

    // 클릭할 때
    public void OnPointerDown(PointerEventData eventData)
    {
        if (SettingManager.Instance.SoundPanel.activeSelf || SettingManager.Instance.ReCheckPanel.activeSelf)
        {
            if (imageComponent != null)
            {
                imageComponent.raycastTarget = false;
            }
            return;
        }

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
