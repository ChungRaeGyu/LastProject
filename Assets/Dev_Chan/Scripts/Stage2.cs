using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2 : MonoBehaviour
{
    public GameObject stageBtn;
    public int num;
    int stage;

    // Start is called before the first frame update
    void Start()
    {
        stage = GameManager_chan2.Instance.stageLevel;

        if (stage == 1)
        {
            if (num == 0)
                stageBtn.SetActive(true);
            else
                stageBtn.SetActive(false);
        }
        if (stage == 2)
        {
            if (num == 1 || num == 2)
                stageBtn.SetActive(true);
            else
                stageBtn.SetActive(false);
        }
        if (stage == 3)
        {
            if (num == 3 || num == 4)
                stageBtn.SetActive(true);
            else
                stageBtn.SetActive(false);
        }
        if (stage == 4)
        {
            if (num == 5)
                stageBtn.SetActive(true);
            else
                stageBtn.SetActive(false);
        }
        else
            stageBtn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
