using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Monster : MonsterCharacter
{
    
    public Slider healthSlider;
    public Button turnEndButton;

    private bool battleOnGoing = true;

    private void Start()
    {
        turnEndButton.onClick.AddListener(TurnEndButton);

        if (healthSlider != null)
        {
            healthSlider.maxValue = monsterStats.maxhealth;
            healthSlider.value = currenthealth;
        }
    }

    void TurnEndButton()
    {
        if (battleOnGoing)
        {
            AttackCharacter(monsterStats); // attack monster
        }

        GameManager.instance.player.InitializeCost();
    }

    public void AttackCharacter(MonsterStats target)
    {
        int damage = monsterStats.attackPower;

            if (Random.value < 0.35f)
            {
                target.maxhealth -= damage;
                damage *= 2;
                Debug.Log($"{this.name} �� ũ��Ƽ�� �������� ������!");
            }

            target.maxhealth -= damage;
            Debug.Log($"{this.name} �� �������� ������!");
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