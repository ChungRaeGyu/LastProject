using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string lastchar = gameObject.name.Substring(gameObject.name.Length - 1);
        int num = int.Parse(lastchar);

        gameObject.GetComponent<Button>().enabled = GameManager_chan.Instance.clearCheck[num-1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
