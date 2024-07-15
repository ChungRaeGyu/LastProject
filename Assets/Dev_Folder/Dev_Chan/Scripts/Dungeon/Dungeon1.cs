using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dungeon1 : Dungeon
{
    void Start()
    {
        SaveManager.Instance.playerPosition = stageNum[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveManager.Instance.playerPosition == stageNum[0].transform.position)
        {
            for (int i = 0; i<stageBtn.Length; i++)
            {
                if( i == 0 || i == 1 || i == 2)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[1].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 1 || i == 3 || i == 4)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[2].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 4 || i == 5 || i == 6)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[3].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 1 || i == 6 || i == 7)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[4].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 4 || i == 8)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[5].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 5 || i == 8 || i == 9)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[6].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 9 || i == 10 || i == 11)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[7].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 5 || i == 11 || i == 12)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[8].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 6 || i == 12)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[9].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 9 || i == 13)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[10].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 10 || i == 13 || i == 14)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[11].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 14 || i == 15 || i == 16)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[12].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 10 || i == 16 || i == 17)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[13].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 11 || i == 17)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[14].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 14)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[15].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 15)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[16].transform.position)
        {
            //Clear ÆÐ³Î
        }
        if (SaveManager.Instance.playerPosition == stageNum[17].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 15)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
        if (SaveManager.Instance.playerPosition == stageNum[18].transform.position)
        {
            for (int i = 0; i < stageBtn.Length; i++)
            {
                if (i == 16)
                {
                    stageBtn[i].SetActive(true);
                }
                else
                {
                    stageBtn[i].SetActive(false);
                }
            }
        }
    }

    public void Warp1()
    {
        SaveManager.Instance.playerPosition = stageNum[13].transform.position;
    }

    public void Warp2()
    {
        SaveManager.Instance.playerPosition = stageNum[9].transform.position;
    }
}
