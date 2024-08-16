using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DescriptionHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject descriptionPanel; // ���� �г�
    public TMP_Text descriptionText; // ���� �ؽ�Ʈ
    public Transform descriptionPosition; // ��ġ ����
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

    // Ŭ���� ��
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

    // Ŭ���� �� ��
    public void OnPointerUp(PointerEventData eventData)
    {
        HideDescription();
    }

    // ���� �г� �����ֱ�
    private void ShowDescription()
    {
        descriptionPanel.SetActive(true);
        descriptionPanel.transform.position = descriptionPosition.position;
    }

    // ���� �г� �����
    private void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
}
