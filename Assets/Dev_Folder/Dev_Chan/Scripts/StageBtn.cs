using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageBtn : MonoBehaviour
{
    public StageManager sm;
    public Vector3 pos;

    private void Start()
    {
        pos = this.gameObject.transform.position;
    }
    public void Battle()
    {
        GameManager_chan.Instance.nowPosition = pos;
        SceneManager.LoadScene(3);
    }

    public void Victory()
    {
        Debug.Log(GameManager_chan.Instance.stageLevel);
        //GameManager_chan.Instance.clearCheck[GameManager_chan.Instance.stageLevel] = true; 
        GameManager_chan.Instance.stageLevel += 1;
        SceneManager.LoadScene(1);
    }
}
