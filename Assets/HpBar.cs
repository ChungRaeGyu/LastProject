using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    public Transform hpBarPos;

    //private void Start()
    //{
    //    if (hpBarPos != null)
    //        hpBarPos = GameManager.instance.player.transform.Find("HpBarPos");
    //}

    private void Update()
    {
        transform.position = hpBarPos.position;
    }
}
