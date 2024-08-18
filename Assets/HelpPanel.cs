using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    public List<GameObject> pages; // 도움말 페이지들
    public GameObject prevButton; // 이전 페이지 버튼

    private int currentPageIndex = 0;

    // 오브젝트가 활성화될 때 호출됨
    void OnEnable()
    {
        // 첫 번째 페이지를 표시하고 나머지를 숨김
        currentPageIndex = 0;
        ShowPage(currentPageIndex);
        UpdateButtonStates();
    }

    // 다음 페이지로 이동
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

            // 마지막 페이지인 경우 오브젝트를 비활성화
            gameObject.SetActive(false);
        }
    }

    // 이전 페이지로 이동
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

    // 특정 페이지를 보여주고 나머지는 숨김
    private void ShowPage(int pageIndex)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == pageIndex);
        }
    }

    // 버튼의 활성화 상태를 업데이트
    private void UpdateButtonStates()
    {
        // 첫 페이지라면 이전 버튼을 숨기고, 그렇지 않으면 보여줌
        if (prevButton != null)
        {
            prevButton.gameObject.SetActive(currentPageIndex > 0);
        }
    }
}
