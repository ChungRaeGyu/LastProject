using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    public float zoomScale = 1.5f; // 확대 배율
    public float zoomDuration = 0.25f; // 축소 애니메이션 지속 시간

    private Vector3 originalScale; // 원래 크기
    private Vector3 originalPosition; // 원래 위치
    private Quaternion originalRotation; // 원래 회전
    private int originalSiblingIndex; // 초기 순서 저장 변수
    private bool isZoomed = false; // 확대 상태 체크

    private void Start()
    {
        originalScale = transform.localScale;
        originalSiblingIndex = transform.GetSiblingIndex();
    }

    private void OnMouseEnter()
    {
        ZoomIn();

        if (isZoomed)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.SetAsLastSibling(); // 맨 위로 올리기
        }
    }

    private void OnMouseExit()
    {
        ZoomOut();
    }

    public void ZoomIn()
    {
        if (isZoomed) return;

        transform.localScale = originalScale * zoomScale;

        isZoomed = true;
    }

    public void ZoomOut()
    {
        if (!isZoomed) return;

        transform.localScale = originalScale;
        transform.rotation = originalRotation;
        transform.SetSiblingIndex(originalSiblingIndex); // 초기 순서로 되돌리기

        isZoomed = false;
    }

    public void SetOriginalPosition(Vector3 position, Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
    }

    public void ResetPosition()
    {
        transform.rotation = originalRotation; // 카드 회전 초기화
    }
}
