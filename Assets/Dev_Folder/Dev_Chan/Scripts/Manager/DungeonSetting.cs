using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSetting : MonoBehaviour
{
    public GameObject enterBtn;
    public GameObject lockDungeon;
    public GameObject explain;

    void Start()
    {
        //if (GameObject.FindGameObjectWithTag("Activate"))
        //{
            enterBtn.SetActive(true);
            lockDungeon.SetActive(false);
            explain.SetActive(false);
        //}
        /*if (GameObject.FindGameObjectWithTag("Inactivate"))
        {
            enterBtn.SetActive(false);
            lockDungeon.SetActive(true);
            explain.SetActive(true);
        }*/
    }

    void Update()
    {
        
    }
}
