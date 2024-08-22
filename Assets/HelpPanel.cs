using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    public List<GameObject> pages; // ���� ��������
    public GameObject prevButton; // ���� ������ ��ư

    private int currentPageIndex = 0;

    // ������Ʈ�� Ȱ��ȭ�� �� ȣ���
    void OnEnable()
    {
        // ù ��° �������� ǥ���ϰ� �������� ����
        currentPageIndex = 0;
        ShowPage(currentPageIndex);
        UpdateButtonStates();
    }

    // ���� �������� �̵�
    public void OnNextPage()
    {
        if (currentPageIndex < pages.Count - 1)
        {
            SettingManager.Instance.PlaySound(SettingManager.Instance.CardPassClip);

            currentPageIndex++;
            ShowPage(currentPageIndex);
            UpdateButtonStates();
        }
        else
        {
            SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip2);

            // ������ �������� ��� ������Ʈ�� ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }

    // ���� �������� �̵�
    public void OnPrevPage()
    {
        if (currentPageIndex > 0)
        {
            SettingManager.Instance.PlaySound(SettingManager.Instance.CardPassClip);

            currentPageIndex--;
            ShowPage(currentPageIndex);
            UpdateButtonStates();
        }
    }

    // Ư�� �������� �����ְ� �������� ����
    private void ShowPage(int pageIndex)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == pageIndex);
        }
    }

    // ��ư�� Ȱ��ȭ ���¸� ������Ʈ
    private void UpdateButtonStates()
    {
        // ù ��������� ���� ��ư�� �����, �׷��� ������ ������
        if (prevButton != null)
        {
            prevButton.gameObject.SetActive(currentPageIndex > 0);
        }
    }
}
