using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;
public class BackEndManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var bro = Backend.Initialize();

        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro);
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro);
        }
        Test();
    }

    private void Test()
    {
        throw new NotImplementedException();
    }
}
