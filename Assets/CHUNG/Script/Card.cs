using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardSO cardSO;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("시작");
        transform.gameObject.name = cardSO.cardName;
        gameObject.GetComponent<Image>().sprite = cardSO.Image;
    }
    private void OnEnable() {
        Debug.Log("활성화");
    }
}
