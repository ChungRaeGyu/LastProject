using UnityEngine;

public class CardDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public Player player { get; private set; }

    private void Start()
    {
        player = GameManager.instance.player;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            transform.position = cursorWorldPoint + offset;
        }
    }

    private void OnMouseDown()
    {
        if (player != null && player.stats.IsGaugeFull())
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 cursorWorldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
            offset = transform.position - cursorWorldPoint;
            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        // 카드를 원래 위치로 되돌리거나 카드 사용 스크립트에 이벤트를 전달
        GetComponent<CardUse>().TryUseCard();
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}
