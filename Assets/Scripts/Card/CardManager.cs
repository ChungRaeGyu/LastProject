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
        // 씬이 로드될 때마다 스크립트를 관리
        ManageScriptsByScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 씬 인덱스에 따라 스크립트들을 관리
    private void ManageScriptsByScene(int sceneIndex)
    {
        if (sceneIndex == 1) // 로비 씬
        {
            SetScriptActive(cardDataScript, true);
            SetScriptActive(cardDragScript, false);
            SetScriptActive(cardCollisionScript, false);
            SetScriptActive(cardZoomScript, false);
        }
        else if (sceneIndex == 2) // 보드 씬
        {
            SetScriptActive(cardDataScript, false);
            SetScriptActive(cardDragScript, false);
            SetScriptActive(cardCollisionScript, false);
            SetScriptActive(cardZoomScript, false);
        }
        else if (sceneIndex == 3) // 메인 씬
        {
            SetScriptActive(cardDataScript, false);
            SetScriptActive(cardDragScript, true);
            SetScriptActive(cardCollisionScript, true);
            SetScriptActive(cardZoomScript, true);
        }
    }

    // 스크립트를 활성화하거나 비활성화
    private void SetScriptActive(MonoBehaviour script, bool isActive)
    {
        if (script != null)
        {
            script.enabled = isActive;
        }
    }
}
