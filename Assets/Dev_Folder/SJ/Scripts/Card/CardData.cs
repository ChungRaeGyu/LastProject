using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class CardData : MonoBehaviour
{
    RectTransform transform;
    Vector2 maxSize = new Vector2(5, 7.5f);
    Vector2 minSize = new Vector2(3, 4.5f);
    private void Awake()
    {
        transform = GetComponent<RectTransform>();


        // ��ȯ�� �� ���
        
    }

    private float ConvertRange(float x, int minOrig, int maxOrig, int minNew, int maxNew)
    {
        float abs = Mathf.Abs(x - 1604)+1604;

        float xNorm = (maxOrig - minOrig)/abs;

        // 2�ܰ�: ����ȭ�� ���� ���ο� ������ ��ȯ

        return xNorm;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1) return;
        float newValue = ConvertRange(transform.position.x, -1178, 4386, 3, 5)*1.5f;
        
        transform.localScale= new Vector2(1* newValue, 1.5f* newValue);
        //1564�϶��� �������� 
    }
}
