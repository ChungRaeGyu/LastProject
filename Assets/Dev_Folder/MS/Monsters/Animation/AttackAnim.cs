using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackAnim : MonoBehaviour
{
    public Button moveButton;

    private void Start()
    {
        moveButton.onClick.AddListener(MonsterAttackAnim2);
    }

    public void MonsterAttackAnim(float startPosition, float endPosition, float duration) // 공격 모션
    {
        transform.DOLocalMoveX(startPosition, duration).OnComplete(() =>
        {
            transform.DOLocalMoveX(endPosition, duration);
        });
    }

    public void MonsterAttackAnim2() // 흔들리는 모션
    {
        transform.DOShakePosition(0.5f, new Vector3(10f, 0, 0), 15, 13, false, false);
    }
}
