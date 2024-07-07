using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject battle;
    public GameObject eventStage;
    public GameObject store;
    
    public void Start()
    {
        int randomNum = Random.Range(0, 100);
        
        if(randomNum < 6)
        {
            battle.SetActive(false);
            eventStage.SetActive(false);
            store.SetActive(true);
        }
        else if(randomNum >= 6 && randomNum <= 15)
        {
            battle.SetActive(false);
            eventStage.SetActive(true);
            store.SetActive(false);
        }
        else
        {
            battle.SetActive(true);
            eventStage.SetActive(false);
            store.SetActive(false);
        }
    }
}
