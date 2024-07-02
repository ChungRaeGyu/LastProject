using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnim : MonoBehaviour
{
     public void MonsterAttackAnim()
     {
        transform.DOMoveX(660, 1).OnComplete(() =>
        {
            transform.DOMoveX(700, 1);
        });
     }
}
