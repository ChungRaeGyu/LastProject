using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    public Slider healthSlider;

    private Character character;
    private CharacterStats characterStats;
    private MonsterInfo monsterInfo;
    public Button turnEndButton;

    private bool battleOnGoing = true;

    private void Start()
    {
        characterStats = new CharacterStats();
        monster = new MonsterInfo();

        turnEndButton.onClick.AddListener(TurnEndButton);

        if (healthSlider != null)
        {
            healthSlider.maxValue = stats.maxhealth;
            healthSlider.value = currenthealth;
        }
    }

    void TurnEndButton()
    {
        if (battleOnGoing)
        {
            AttackCharacter(characterStats); // attack monster

            if (character.IsDead())
            {
                Debug.Log("�÷��̾� ���");
                Die();
                battleOnGoing = false;
            }
        }

    }
    public void AttackCharacter(CharacterStats target, bool isEnemy = false)
    {
        int damage = monsterInfo.attackPower;

        if(isEnemy)
        {
           if (Random.value < 0.35f)
           {
                //target.maxhealth -= damage;
                damage *= 2;
                Debug.Log($"{this.name} �� ũ��Ƽ�� �������� ������!");
           }

           target.maxhealth -= damage;
           Debug.Log($"{this.name} �� �������� ������!");
        }

    }

    protected override void Update()
    {
        base.Update();

        if (healthSlider != null)
        {
            healthSlider.value = currenthealth;
        }
    }
}
