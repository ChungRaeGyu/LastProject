using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSystem : MonoBehaviour
{
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
                int randomNormal = Random.Range(0, normalCards.Count);
                ObjSet(obj, randomNormal);
            }
            else if(random<95){
                //희귀카드뽑기
                int randomNormal = Random.Range(0, rarityCards.Count);
                ObjSet(obj, randomNormal);
            }
            else{
                //영웅카드뽑기
                int randomNormal = Random.Range(0, heroCards.Count);
                ObjSet(obj, randomNormal);
            }
        }
    }

    private void ObjSet(GameObject obj, int randomNormal)
    {
        obj.GetComponent<Card>().cardSO = normalCards[randomNormal];
        obj.SetActive(true);
    }
}
