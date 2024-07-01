using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnAttack : MonoBehaviour
{
    private MonStats player;
    private MonStats monster;

    public Image playerHpBar;
    public Image monsterHpBar;

    public Image playerGagueBar;
    public Image monsterGagueBar;

    public Button attackButton;
    public Button turnEndButton;

    private bool battleOnGoing = true;



    void Start()
    {
        player = new MonStats("�÷��̾�", 100, 100, 3, 10, 3, 3);
        monster = new MonStats("����", 50, 50, 0, 10, 2, 2);

        UpdateHpBar(playerHpBar, player);
        UpdateHpBar(monsterHpBar, monster);

        attackButton.onClick.AddListener(AttackButton);
        turnEndButton.onClick.AddListener(TurnEndButton);
        //StartCoroutine(AttackButton());
    }

    void UpdateHpBar(Image HpBar, MonStats monster) // fillAmount hpbar
    {
        HpBar.fillAmount = (float)monster.curHealth / monster.maxHealth;
    }


    void AttackButton()  //yield return new WaitForSeconds(1f); == BreakTime() add example
    {
        if (battleOnGoing)
        {
            player.Attack(monster); // attack monster
            UpdateHpBar(monsterHpBar, monster); // update monster hpbar

            if (!monster.IsAlive())
            {
                Debug.Log("������ �й�");
                battleOnGoing = false;
            }
        }
    }
    void TurnEndButton()
    {
        if (battleOnGoing)
        {
             monster.Attack(player); // attack player twice damage
             UpdateHpBar(playerHpBar, player); // update player hpbar

             if (!player.IsAlive())
             {
                 Debug.Log("�÷��̾��� �й�");
                 battleOnGoing = false;
             }
        }
    }

     private IEnumerator BreakTime()
     {
            yield return new WaitForSeconds(1f);
     }
}
