using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageBtn : MonoBehaviour
{
    public GameManager_chan gm;

    public void Battle()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void Victory()
    {
        Debug.Log(GameManager_chan.Instance.stageLevel);
        GameManager_chan.Instance.clearCheck[GameManager_chan.Instance.stageLevel - 1] = true; 
        
        SceneManager.LoadScene("StageBoardScene");
    }
}
