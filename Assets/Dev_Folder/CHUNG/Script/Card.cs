using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    //카드를 뽑을때, 덱을 로딩할때, 도감을 로딩할 때, 핸드를 볼때
    // cardSO를 새로 설정해준다.
    [Header("PutInScript")]
    public CardSO cardSO;

    Image cardImage;

    Button button;
    public GameObject DrawBoard;
    public DrawSystem drawSystem;
    // Start is called before the first frame update
    private void Awake(){
        cardImage= GetComponent<Image>();
        button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        ButtonIsActive();

    }

    private void ImageSet()
    {
        transform.gameObject.name = cardSO.cardName;
        cardImage.sprite = cardSO.Image;
    }

    private void ButtonIsActive()
    {
        if (transform.root.TryGetComponent<DrawSystem>(out drawSystem))
        {
            button.enabled = true;
            transform.gameObject.name = cardSO.cardName;
            cardImage.sprite = cardSO.defaultImage;
        }
        else
        {
            button.enabled = false;
            ImageSet();
        }
    }

    public void OpenCard(){
        if(drawSystem.tempCardSO.Count==0)return;
        cardSO = drawSystem.tempCardSO.Dequeue();
        ImageSet();
    }
}
