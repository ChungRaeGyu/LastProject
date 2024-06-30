using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    //카드를 뽑을때, 덱을 로딩할때, 도감을 로딩할 때, 핸드를 볼때
    // cardSO를 새로 설정해준다.
    [Header("PutInScript")]
    public CardSO cardSO;

    Image cardImage;
    // Start is called before the first frame update
    private void Awake(){
        cardImage= GetComponent<Image>();
    }
    private void OnEnable() {
        transform.gameObject.name = cardSO.cardName;
        cardImage.sprite = cardSO.Image;
    }
}
