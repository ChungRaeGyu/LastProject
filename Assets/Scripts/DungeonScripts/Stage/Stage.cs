using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public GameObject start;
    public GameObject battle;
    public GameObject warp;
    public GameObject eventStage;
    public GameObject store;
    public GameObject eliteMob;
    public GameObject boss;

    //스테이지 포지션
    public Vector3 stagePosition;
    public Vector3 startPosition;

    public System.Random random = new System.Random();

    public void Start()
    {
        stagePosition = this.gameObject.transform.position;
        startPosition = Dungeon.Instance.stage[((Dungeon.Instance.x - 1) / 2), 0].transform.position;
        Dungeon.Instance.dungeonProgress = (stagePosition.x - startPosition.x) / Dungeon.Instance.dungeonLength * 10;
        Debug.Log(Dungeon.Instance.dungeonProgress);

        //위치에 따라 각종 스테이지 개방
        if (stagePosition == Dungeon.Instance.stage[((Dungeon.Instance.x - 1) / 2), 0].transform.position)
        {
            start.SetActive(true);
        }
        else if (stagePosition == Dungeon.Instance.stage[0, ((Dungeon.Instance.y - 1) / 2)].transform.position)
        {
            warp.SetActive(true);
        }
        else if (stagePosition == Dungeon.Instance.stage[(Dungeon.Instance.x - 1), ((Dungeon.Instance.y - 1) / 2)].transform.position)
        {
            warp.SetActive(true);
        }
        else if (stagePosition == Dungeon.Instance.stage[((Dungeon.Instance.x - 1) / 2), (Dungeon.Instance.y - 1)].transform.position)
        {
            boss.SetActive(true);
        }
        else
        {
            for(int i = 0; i<Dungeon.Instance.x; i++)
            {
                for(int j = 0; j<Dungeon.Instance.y; j++)
                {
                    if(Dungeon.Instance.isStage[i, j] && stagePosition == Dungeon.Instance.stage[i, j].transform.position)
                    {
                        switch (SaveManager.Instance.num[i, j])
                        {
                            case 0: //던전 현재 상황이 50%이상 진입했을 경우
                                if (Dungeon.Instance.dungeonProgress > 50)
                                    eliteMob.SetActive(true); //엘리트몹 스테이지 활성화
                                else
                                    battle.SetActive(true); //전투스테이지 활성화
                                break;
                            case 1:
                            case 2:
                                store.SetActive(true);
                                break;
                            case 3:
                            case 4:
                            case 5:
                                eventStage.SetActive(true);
                                break;
                            default:
                                battle.SetActive(true);
                                break;
                        }
                    }
                }
            }
        }
    }

    public void BattleBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        if (gameObject.name == "BossBtn")
        {
            SaveManager.Instance.isBossStage = true;
        }
        SaveManager.Instance.playerPosition = stagePosition - transform.parent.position;
        SaveManager.Instance.playerPosition.z = 0;
        Dungeon.Instance.MonsterSpawn();
        DataManager.Instance.SuffleDeckList();
        LoadingSceneManager.LoadScene(3);
        Debug.Log("전투를 개시합니다.");
    }

    public void WarpBtn()
    {
        if (stagePosition == Dungeon.Instance.stage[0, ((Dungeon.Instance.y - 1) / 2)].transform.position)
        {
            SaveManager.Instance.playerPosition = Dungeon.Instance.stage[Dungeon.Instance.x - 1, ((Dungeon.Instance.y - 1) / 2)].transform.position;
        }
        else if (stagePosition == Dungeon.Instance.stage[Dungeon.Instance.x - 1, ((Dungeon.Instance.y - 1) / 2)].transform.position)
        {
            SaveManager.Instance.playerPosition = Dungeon.Instance.stage[0, ((Dungeon.Instance.y - 1) / 2)].transform.position;
        }
    }

    public void EventBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        SaveManager.Instance.playerPosition = stagePosition;

        // 랜덤으로 이벤트 패널을 띄워줌
        DungeonManager.Instance.eventManager.ShowRandomEvent();
    }


    public void StoreBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        SaveManager.Instance.playerPosition = stagePosition;
        LoadingSceneManager.LoadScene(4);
    }

    public void EliteMobBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        // 엘리트 스테이지를 확인
        SaveManager.Instance.isEliteStage = true;

        SaveManager.Instance.playerPosition = stagePosition;
        Dungeon.Instance.MonsterSpawn();
        DataManager.Instance.SuffleDeckList();
        LoadingSceneManager.LoadScene(3);
        Debug.Log("전투를 개시합니다.");
    }

    public void BossBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        // 보스 스테이지를 확인
        SaveManager.Instance.isBossStage = true;

        SaveManager.Instance.playerPosition = stagePosition;
        Dungeon.Instance.MonsterSpawn();
        DataManager.Instance.SuffleDeckList();
        LoadingSceneManager.LoadScene(3);
        Debug.Log("전투를 개시합니다.");
    }
}
