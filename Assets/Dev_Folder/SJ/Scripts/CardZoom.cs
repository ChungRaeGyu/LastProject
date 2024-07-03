using UnityEngine;

public class CardZoom : MonoBehaviour
{
    public float zoomScale = 2.0f; // 확대 배율
    public Vector3 zoomOffset = new Vector3(0, 5, 10); // 확대된 카드의 위치 오프셋
    private Vector3 originalScale; // 원래 크기
    private Vector3 originalPosition; // 원래 위치
    private int originalOrder; // 원래의 Order in Layer
    private bool isZoomed = false; // 확대 상태 체크

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalOrder = spriteRenderer.sortingOrder;
        }
    }

    private void OnMouseEnter()
    {
        ZoomIn();
    }

    private void OnMouseExit()
    {
        ZoomOut();
    }

    private void ZoomIn()
    {
        if (isZoomed) return;

        transform.localScale = originalScale * zoomScale;
        transform.position += zoomOffset;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 100;
        }

        isZoomed = true;
    }

    private void ZoomOut()
    {
        if (!isZoomed) return;

        transform.localScale = originalScale;
        transform.position = originalPosition;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = originalOrder;
        }

        isZoomed = false;
    }
}
