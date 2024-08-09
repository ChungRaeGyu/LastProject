using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public Vector3 stagePosition;

    public System.Random random = new System.Random();

    public int x, y;

    public void Start()
    {
        stagePosition = this.gameObject.transform.position;
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
        SceneFader.instance.LoadSceneWithFade(3);
        Debug.Log("������ �����մϴ�.");
        Dungeon.Instance.GetValue(x, y);
        Debug.Log($"{x},{y}");
    }

    public void WarpBtn()
    {
        //SaveManager.Instance.playerPosition = stagePosition;

        if (stagePosition == Dungeon.Instance.stage[0, ((Dungeon.Instance.y - 1) / 2)].transform.position)
        {
            //SaveManager.Instance.playerPosition = Dungeon.Instance.stage[Dungeon.Instance.x - 1, ((Dungeon.Instance.y - 1) / 2)].transform.position;
            Dungeon.Instance.GetValue(Dungeon.Instance.x - 1, ((Dungeon.Instance.y - 1) / 2));
        }
        else if (stagePosition == Dungeon.Instance.stage[Dungeon.Instance.x - 1, ((Dungeon.Instance.y - 1) / 2)].transform.position)
        {
            //SaveManager.Instance.playerPosition = Dungeon.Instance.stage[0, ((Dungeon.Instance.y - 1) / 2)].transform.position;
            Dungeon.Instance.GetValue(0, ((Dungeon.Instance.y - 1) / 2));

        }

        SceneFader.instance.LoadSceneWithFade(2);

    }

    public void EventBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        SaveManager.Instance.playerPosition = stagePosition;

        // �������� �̺�Ʈ �г��� �����
        DungeonManager.Instance.eventManager.ShowRandomEvent();
        Dungeon.Instance.GetValue(x, y);

    }


    public void StoreBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        SaveManager.Instance.playerPosition = stagePosition;
        SceneFader.instance.LoadSceneWithFade(4);
        Dungeon.Instance.GetValue(x, y);

    }

    public void EliteMobBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        // ����Ʈ ���������� Ȯ��
        SaveManager.Instance.isEliteStage = true;

        SaveManager.Instance.playerPosition = stagePosition;
        Dungeon.Instance.MonsterSpawn();
        DataManager.Instance.SuffleDeckList();
        SceneFader.instance.LoadSceneWithFade(3);
        Debug.Log("������ �����մϴ�.");
        Dungeon.Instance.GetValue(x, y);

    }

    public void BossBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        // ���� ���������� Ȯ��
        SaveManager.Instance.isBossStage = true;

        SaveManager.Instance.playerPosition = stagePosition;
        Dungeon.Instance.MonsterSpawn();
        DataManager.Instance.SuffleDeckList();
        SceneFader.instance.LoadSceneWithFade(3);
        Debug.Log("������ �����մϴ�.");
        Dungeon.Instance.GetValue(x, y);

    }

    public void SetValue(int x, int y) 
    {
        this.x = x;
        this.y = y;
        Debug.Log($"{x},{y}");
    }


}
