using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public Vector3 stagePosition;

    public System.Random random = new System.Random();

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
    }

    public void WarpBtn()
    {
        //SaveManager.Instance.playerPosition = stagePosition;

        if (stagePosition == Dungeon.Instance.stage[0, ((Dungeon.Instance.y - 1) / 2)].transform.position)
        {
            SaveManager.Instance.playerPosition = Dungeon.Instance.stage[Dungeon.Instance.x - 1, ((Dungeon.Instance.y - 1) / 2)].transform.position;
        }
        else if (stagePosition == Dungeon.Instance.stage[Dungeon.Instance.x - 1, ((Dungeon.Instance.y - 1) / 2)].transform.position)
        {
            SaveManager.Instance.playerPosition = Dungeon.Instance.stage[0, ((Dungeon.Instance.y - 1) / 2)].transform.position;
        }

        SceneFader.instance.LoadSceneWithFade(2);
    }

    public void EventBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        SaveManager.Instance.playerPosition = stagePosition;

        // �������� �̺�Ʈ �г��� �����
        DungeonManager.Instance.eventManager.ShowRandomEvent();
    }


    public void StoreBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        SaveManager.Instance.playerPosition = stagePosition;
        SceneFader.instance.LoadSceneWithFade(4);
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
    }
}
