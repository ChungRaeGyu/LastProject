using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public GameObject stageBtn;
    public int num;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("num" + num);
        //stageBtn.SetActive(GameManager_chan.Instance.clearCheck[num]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
