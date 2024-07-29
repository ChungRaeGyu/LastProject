using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Test : MonoBehaviour
{
    public GameObject[,] stage;
    public GameObject stagePrefab;


    public Canvas stageCanvas;

    public int[,] isStage;

    public int x;
    public int y;

    // Start is called before the first frame update
    void Start()
    {
        isStage = new int[,] { { 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0 },
                               { 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 },
                               { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 },
                               { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
                               { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 },
                               { 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 },
                               { 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0 } };


        stage = new GameObject[x, y];

        for (int a = 0; a < x; a++)
        {
            for (int b = 0; b < y; b++)
            {

                if (isStage[a, b] == 1)
                {
                    stage[a, b] = Instantiate(stagePrefab, stageCanvas.transform);
                    stage[a, b].transform.position = new Vector2(a, b);
                }

            }
        }
        SaveManager.Instance.playerPosition = stage[x/2,0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (SaveManager.Instance.playerPosition == stage[0].transform.position)
        {
            // (stageNum[i, j] == 0)
            {
                //stageNum[i + 1, j];
                //stageNum[i + 2, j];
                //stageNum[i + 1, j + 1];
            }
        }
    }
}
