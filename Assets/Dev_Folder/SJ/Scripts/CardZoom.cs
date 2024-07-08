using System.Collections;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    public float zoomScale = 2.0f; // 확대 배율
    public Vector3 zoomOffset = new Vector3(0, 5, 10); // 확대된 카드의 위치 오프셋
    public float zoomDuration = 0.25f; // 축소 애니메이션 지속 시간

    private Vector3 originalScale; // 원래 크기
    private Vector3 originalPosition; // 원래 위치
    private Quaternion originalRotation; // 원래 회전
    private int originalOrder; // 원래의 Order in Layer
    private bool isZoomed = false; // 확대 상태 체크

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
        transform.position = originalPosition; // 카드 위치 초기화
        transform.rotation = originalRotation; // 카드 회전 초기화
    }
}
