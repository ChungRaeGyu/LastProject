using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    //카드를 뽑을때, 덱을 로딩할때, 도감을 로딩할 때, 핸드를 볼때
    // cardSO를 새로 설정해준다.
    [Header("PutInScript")]
    public CardBasic cardObj;

    Image cardImage;

    Button button;
    public GameObject DrawBoard;
    public DrawSystem drawSystem;

    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI descriptionLabel;
    public TextMeshProUGUI costLabel;
    // Start is called before the first frame update
    private void Awake(){
        cardImage= GetComponent<Image>();
        button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        ButtonIsActive();

    }

    public void ImageSet()
    {
        transform.gameObject.name = cardObj.cardName;
        cardImage.sprite = cardObj.image;
        UpdateUI();
    }

    private void ButtonIsActive()
    {
        if (transform.root.TryGetComponent<DrawSystem>(out drawSystem))
        {
            button.enabled = true;
            transform.gameObject.name = cardObj.cardName;
            cardImage.sprite = cardObj.defaultImage;
        }
        else
        {
            button.enabled = false;
            ImageSet();
        }
    }

    public void OpenCard(){
        if(drawSystem.tempCardBasic.Count==0)return;
        cardObj = drawSystem.tempCardBasic.Dequeue();
        ImageSet();
    }

    void UpdateUI()
    {
        nameLabel.text = cardObj.cardName;
        descriptionLabel.text = cardObj.description;
        costLabel.text = cardObj.cost.ToString();
    }
}
