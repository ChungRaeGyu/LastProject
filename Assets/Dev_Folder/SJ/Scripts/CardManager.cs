using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CardScriptManager : MonoBehaviour
{
    [Header("scripts")]
    public MonoBehaviour cardDataScript;
    public MonoBehaviour cardDragScript;
    public MonoBehaviour cardCollisionScript;
    public MonoBehaviour cardZoomScript;

    private void Awake()
    {
        cardDataScript = GetComponent<CardData>();
        cardDragScript = GetComponent<CardDrag>();
        cardCollisionScript = GetComponent<CardCollision>();
        cardZoomScript = GetComponent<CardZoom>();
        // ���� �ε�� ������ ��ũ��Ʈ�� ����
        ManageScriptsByScene(SceneManager.GetActiveScene().buildIndex);
    }

    // �� �ε����� ���� ��ũ��Ʈ���� ����
    private void ManageScriptsByScene(int sceneIndex)
    {
        if (sceneIndex == 1) // �κ� ��
        {
            SetScriptActive(cardDataScript, true);
            SetScriptActive(cardDragScript, false);
            SetScriptActive(cardCollisionScript, false);
            SetScriptActive(cardZoomScript, false);
        }
        else if (sceneIndex == 2) // ���� ��
        {
            SetScriptActive(cardDataScript, false);
            SetScriptActive(cardDragScript, false);
            SetScriptActive(cardCollisionScript, false);
            SetScriptActive(cardZoomScript, false);
        }
        else if (sceneIndex == 3) // ���� ��
        {
            SetScriptActive(cardDataScript, false);
            SetScriptActive(cardDragScript, true);
            SetScriptActive(cardCollisionScript, true);
            SetScriptActive(cardZoomScript, true);
        }
    }

    // ��ũ��Ʈ�� Ȱ��ȭ�ϰų� ��Ȱ��ȭ
    private void SetScriptActive(MonoBehaviour script, bool isActive)
    {
        if (script != null)
        {
            script.enabled = isActive;
        }
    }
}
