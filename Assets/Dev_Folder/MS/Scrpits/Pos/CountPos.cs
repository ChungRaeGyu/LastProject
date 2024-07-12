using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountPos : MonoBehaviour
{
    private int countSkill = 8; // 패턴에 사용될 카운트 ex] 3턴 뒤에 무슨무슨 공격을 한다. [예시]
    private int countSkills = 5;

    public GameObject countImage; // 화면에 보여질 공격 아이콘
    public GameObject countImage2; // 화면에 보여질 공격 아이콘2

    public TMP_Text countText; // 패턴에 사용되는 카운트
    public TMP_Text countText2;

    public GameObject countButton; // 턴 종료버튼 대용 [예시]
    public CountPos countPosition;



    private void Start() // 보여지는 카운트를 -- 로 수정해서 0이되면 아이콘이 꺼지게 끔 한다.
    {
        // countPos = transform;

        if (countText != null || countText2 != null)
        {
            Debug.Log("카운트 start");
            UpdateCountText();
            UpdateButtonVisibility();
        }
        //if (countText2 != null)
        //{
        //    Debug.Log("카운트 start");
        //    UpdateCountText();
        //    UpdateButtonVisibility();
        //}
    }

    public void OnBtnClick()
    {
        if (countPosition != null)
        {
            Debug.Log("카운트 OnBtnClick");
            OnCountSkill();
        }
        else
        {
            Debug.Log("카운트 OnBtnClick 에러");
        }
    }

    public void OnCountSkill()  // OnCountSkill 대신 패턴에 사용될 패턴의 카운트를 사용한다
    {
        Debug.Log("카운트 OnCountSkill");
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

    private void UpdateCountText() //start 와 Monsterurn 끝에 들어가면 된다
    {
        if (countText != null || countText2 != null)
        {
            countText.text = countSkill.ToString();
            countText2.text = countSkill.ToString();
            Debug.Log("카운트 CountText2");
            Debug.Log("카운트 CountText1");
        }
        //if (countText2 != null) 
        //{
        //    countText2.text = countSkill.ToString();
        //    Debug.Log("카운트 CountText2");
        //}
    }

    private void UpdateButtonVisibility() // start 와 Monsterurn 끝에 들어가면 된다
    {
        if (countImage != null || countImage2 != null)
        {
            countImage.SetActive(countSkill <= 7 && countSkill > 0); //카운트가 x이되면 이미지가 사라진다\
            countImage2.SetActive(countSkills <= 4 && countSkills > 0);
        }
        //if (countImage2 != null)
        //{
        //    countImage2.SetActive(countSkills <= 4 && countSkills > 0);
        //}
    }
}
