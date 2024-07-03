using UnityEngine;

public class CardZoom : MonoBehaviour
{
    public float zoomScale = 2.0f; // Ȯ�� ����
    public Vector3 zoomOffset = new Vector3(0, 5, 10); // Ȯ��� ī���� ��ġ ������
    private Vector3 originalScale; // ���� ũ��
    private Vector3 originalPosition; // ���� ��ġ
    private int originalOrder; // ������ Order in Layer
    private bool isZoomed = false; // Ȯ�� ���� üũ

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
