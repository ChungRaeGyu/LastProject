using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnAttack : MonoBehaviour
{
    private MonsterStats player;
    private MonsterStats monster;

    public Image playerHpBar;
    public Image monsterHpBar;

    public Image playerGagueBar;
    public Image monsterGagueBar;

    public Button attackButton;

    private int playerAttackCount = 0;
    private bool battleOnGoing = true;



    void Start()
    {
        player = new MonsterStats("플레이어", 100, 100, 3, 10, 3, 3);
        monster = new MonsterStats("몬스터", 50, 50, 0, 3, 2, 2);

        UpdateHpBar(playerHpBar, player);
        UpdateHpBar(monsterHpBar, monster);

        UpdateGaugeBar(monsterGagueBar, monster);

        attackButton.onClick.AddListener(AttackButton);
        //StartCoroutine(AttackButton());
    }

    void UpdateHpBar(Image HpBar, MonsterStats monster) // fillAmount hpbar
    {
        HpBar.fillAmount = (float)monster.curHealth / monster.maxHealth;
    }

    void UpdateGaugeBar(Image GaugeBar, MonsterStats monster)
    {
        GaugeBar.fillAmount = (float)playerAttackCount / 3;
    }

    void AttackButton()  //yield return new WaitForSeconds(1f); == BreakTime() add example
    {
        if (battleOnGoing)
        {
            if (playerAttackCount < 3)
            {
                player.Attack(monster); // attack monster
                UpdateHpBar(monsterHpBar, monster); // update monster hpbar

                playerAttackCount++;

                UpdateGaugeBar(monsterGagueBar, monster);

                if (!monster.IsAlive())
                {
                    Debug.Log("몬스터의 패배");
                    battleOnGoing = false;
                }
            }

            if(playerAttackCount >= 3 && battleOnGoing)
            {
                monster.Attack(player, true); // attack player twice damage
                UpdateHpBar(playerHpBar, player); // update player hpbar

                playerAttackCount = 0;

                UpdateGaugeBar(monsterGagueBar, monster);

                if (!player.IsAlive())
                {
                    Debug.Log("플레이어의 패배");
                    battleOnGoing = false;
                }
            }
        }
    }

    private IEnumerator BreakTime()
    {
        yield return new WaitForSeconds(1f);
    }
}
