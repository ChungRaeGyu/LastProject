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
        //기본값으로 모든 던전 잠금
        enterBtn.SetActive(false);
        lockDungeon.SetActive(true);
        explain.SetActive(true);
    }

    void Update()
    {
        //던전에 입장 가능한 경우 잠금 해제
        switch (gameObject.name)
        {
            case "01_Start_Dungeon":
                if (SaveManager.Instance.accessibleDungeon[0] == true)
                {
                    enterBtn.SetActive(true);
                    lockDungeon.SetActive(false);
                    explain.SetActive(false);
                }
                break;

            case "02_Dungeon":
                if (SaveManager.Instance.accessibleDungeon[1] == true)
                {
                    enterBtn.SetActive(true);
                    lockDungeon.SetActive(false);
                    explain.SetActive(false);
                }
                break;

            case "03_Dungeon":
                if (SaveManager.Instance.accessibleDungeon[2] == true)
                {
                    enterBtn.SetActive(true);
                    lockDungeon.SetActive(false);
                    explain.SetActive(false);
                }
                break;

            case "04_Dungeon":
                if (SaveManager.Instance.accessibleDungeon[3] == true)
                {
                    enterBtn.SetActive(true);
                    lockDungeon.SetActive(false);
                    explain.SetActive(false);
                }
                break;

            case "05_Dungeon":
                if (SaveManager.Instance.accessibleDungeon[4] == true)
                {
                    enterBtn.SetActive(true);
                    lockDungeon.SetActive(false);
                    explain.SetActive(false);
                }
                break;
        }
    }
}
