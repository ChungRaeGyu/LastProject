using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSystem : MonoBehaviour
{
    //DrawSystem에 달려있습니다.
    public List<CardSO> normalCards; //보통;
    public List<CardSO> rarityCards; //희귀;
    public List<CardSO> heroCards;//영웅카드;

    public List<GameObject> drawObj; //뽑기 개수 만큼 넣어줄 것이다. , 화면에 오브젝트로 보여주기 위해 넣었다.


    public void DrawingCardBtn(){
        foreach (GameObject obj in drawObj)
        {
            int random = Random.Range(1,100);
            if(random<80)
            {
                //노말카드
                int randomCard = Random.Range(0, normalCards.Count);
                ObjSet(obj, randomCard);
            }
            else if(random<95){
                //희귀카드뽑기
                int randomCard = Random.Range(0, rarityCards.Count);
                ObjSet(obj, randomCard);
            }
            else{
                //영웅카드뽑기
                int randomCard = Random.Range(0, heroCards.Count);
                ObjSet(obj, randomCard);
            }
        }
    }

    private void ObjSet(GameObject obj, int randomCard)
    {
        obj.GetComponent<Card>().cardSO = normalCards[randomCard];
        obj.SetActive(true);
    }

    //패널 닫기 
    public void ClosePanel(){
        SaveCardInBook();
    }

    //Book(도감)으로 넣어준다. 그리고 카드를 다 초기화 시켜주기
    private void SaveCardInBook()
    {            
        //초기화
        foreach(GameObject obj in drawObj){
            Card card = obj.GetComponent<Card>();
            card.cardSO.currentCount++;
            card.cardSO=null;
            obj.SetActive(false);
        }
    }
}
