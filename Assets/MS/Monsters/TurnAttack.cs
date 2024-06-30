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
    public Button attackButton;

    private bool playerTurn = true;
    private bool battleOnGoing = true;

    void Start()
    {
        player = new MonsterStats("플레이어", 100, 100, 3, 10, 3, 3);
        monster = new MonsterStats("몬스터", 30, 30, 0, 3, 2, 2);

        UpdateHpBar(playerHpBar, player);
        UpdateHpBar(monsterHpBar, monster);

        attackButton.onClick.AddListener(AttackButton);
        //StartCoroutine(AttackButton());
    }

    void UpdateHpBar(Image HpBar, MonsterStats monster)
    {
        HpBar.fillAmount = (float)monster.curHealth / monster.maxHealth;
    }
    void AttackButton()
    {
        if (battleOnGoing)
        {
            if (playerTurn)
            {
                player.Attack(monster);
                UpdateHpBar(monsterHpBar, monster);
                //yield return new WaitForSeconds(1f);

                if (!monster.IsAlive())
                {
                    Debug.Log("몬스터가 짐");
                    battleOnGoing = false;
                }
            }
            else
            {
                monster.Attack(player);
                UpdateHpBar(playerHpBar, player);
                //yield return new WaitForSeconds(1f);
                if (!player.IsAlive())
                {
                    Debug.Log("플레이어가 짐");
                    battleOnGoing = false;
                }
            }
            
            playerTurn = !playerTurn;
        }
    }
}
