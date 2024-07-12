using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountPos : MonoBehaviour
{
    private int countSkill = 8; // ���Ͽ� ���� ī��Ʈ ex] 3�� �ڿ� �������� ������ �Ѵ�. [����]
    private int countSkills = 5;

    public GameObject countImage; // ȭ�鿡 ������ ���� ������
    public GameObject countImage2; // ȭ�鿡 ������ ���� ������2

    public TMP_Text countText; // ���Ͽ� ���Ǵ� ī��Ʈ
    public TMP_Text countText2;

    public GameObject countButton; // �� �����ư ��� [����]
    public CountPos countPosition;



    private void Start() // �������� ī��Ʈ�� -- �� �����ؼ� 0�̵Ǹ� �������� ������ �� �Ѵ�.
    {
        // countPos = transform;

        if (countText != null || countText2 != null)
        {
            Debug.Log("ī��Ʈ start");
            UpdateCountText();
            UpdateButtonVisibility();
        }
        //if (countText2 != null)
        //{
        //    Debug.Log("ī��Ʈ start");
        //    UpdateCountText();
        //    UpdateButtonVisibility();
        //}
    }

    public void OnBtnClick()
    {
        if (countPosition != null)
        {
            Debug.Log("ī��Ʈ OnBtnClick");
            OnCountSkill();
        }
        else
        {
            Debug.Log("ī��Ʈ OnBtnClick ����");
        }
    }

    public void OnCountSkill()  // OnCountSkill ��� ���Ͽ� ���� ������ ī��Ʈ�� ����Ѵ�
    {
        Debug.Log("ī��Ʈ OnCountSkill");
        countSkill--;
        countSkills--;
        if (countSkill == 0 || countSkills == 0)
        {
            countSkill = 8;
            countSkills = 5;
        }
        //if (countSkills == 0)
        //{
        //    countSkills = 5;
        //}
        UpdateCountText();
        UpdateButtonVisibility();
    }

    private void UpdateCountText() //start �� Monsterurn ���� ���� �ȴ�
    {
        if (countText != null || countText2 != null)
        {
            countText.text = countSkill.ToString();
            countText2.text = countSkill.ToString();
            Debug.Log("ī��Ʈ CountText2");
            Debug.Log("ī��Ʈ CountText1");
        }
        //if (countText2 != null) 
        //{
        //    countText2.text = countSkill.ToString();
        //    Debug.Log("ī��Ʈ CountText2");
        //}
    }

    private void UpdateButtonVisibility() // start �� Monsterurn ���� ���� �ȴ�
    {
        if (countImage != null || countImage2 != null)
        {
            countImage.SetActive(countSkill <= 7 && countSkill > 0); //ī��Ʈ�� x�̵Ǹ� �̹����� �������\
            countImage2.SetActive(countSkills <= 4 && countSkills > 0);
        }
        //if (countImage2 != null)
        //{
        //    countImage2.SetActive(countSkills <= 4 && countSkills > 0);
        //}
    }
}
