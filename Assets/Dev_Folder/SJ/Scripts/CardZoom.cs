using System.Collections;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    public float zoomScale = 2.0f; // Ȯ�� ����
    public Vector3 zoomOffset = new Vector3(0, 5, 10); // Ȯ��� ī���� ��ġ ������
    public float zoomDuration = 0.25f; // ��� �ִϸ��̼� ���� �ð�

    private Vector3 originalScale; // ���� ũ��
    private Vector3 originalPosition; // ���� ��ġ
    private Quaternion originalRotation; // ���� ȸ��
    private int originalOrder; // ������ Order in Layer
    private bool isZoomed = false; // Ȯ�� ���� üũ

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        originalScale = transform.localScale;
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
        transform.rotation = originalRotation;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = originalOrder;
        }

        isZoomed = false;
    }

    private IEnumerator AnimateZoomOut()
    {
        float elapsedTime = 0f;
        Vector3 startingScale = transform.localScale;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < zoomDuration)
        {
            transform.localScale = Vector3.Lerp(startingScale, originalScale, elapsedTime / zoomDuration);
            transform.position = Vector3.Lerp(startingPosition, originalPosition, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = originalOrder;
        }
    }

    public void SetOriginalPosition(Vector3 position, Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
    }

    public void ResetPosition()
    {
        transform.position = originalPosition; // ī�� ��ġ �ʱ�ȭ
        transform.rotation = originalRotation; // ī�� ȸ�� �ʱ�ȭ
    }
}
