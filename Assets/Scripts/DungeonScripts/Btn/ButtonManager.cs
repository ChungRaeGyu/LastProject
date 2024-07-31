using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // ���� ����� ���ư��� ��ư
    public void BoardBtn()
    {
        Time.timeScale = 1.0f;
        SaveManager.Instance.accessDungeon = false;
        SceneManager.LoadScene(2);
    }

    //�κ�� ���ư��� ��ư
    public void HomeBtn()
    {
        Time.timeScale = 1.0f;
        SaveManager.Instance.accessDungeon = false;
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene("Lobby");
    }

    //�������� Ŭ�����ϸ� �������� ����� ���ư��� ��ư
    public void StageClear()
    {
        DungeonManager.Instance.eventScene.SetActive(false);
    }
}
