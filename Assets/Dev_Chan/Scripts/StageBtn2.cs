using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageBtn2 : MonoBehaviour
{
    public void Battle()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void Victory()
    {
        Debug.Log(GameManager_chan2.Instance.stageLevel);
        GameManager_chan2.Instance.stageLevel += 1;

        SceneManager.LoadScene("StageBoardScene");
    }
}
