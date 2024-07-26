using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject descriptionPanel; // ���� �г�
    public TMP_Text descriptionText; // ���� �ؽ�Ʈ
    public Transform descriptionPosition; // ��ġ ����

    private void Start()
    {
        HideDescription();
    }

    // Ŭ���� ��
    public void OnPointerDown(PointerEventData eventData)
    {
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
