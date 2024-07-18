using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float zoomScale = 1.5f; // 확대 배율
    public float zoomDuration = 0.25f; // 축소 애니메이션 지속 시간

    private Vector3 originalScale; // 원래 크기
    private Vector3 originalPosition; // 원래 위치
    private Quaternion originalRotation; // 원래 회전
    private int originalSiblingIndex; // 초기 순서 저장 변수
    private bool isZoomed = false; // 확대 상태 체크

    private static CardZoom currentlyZoomedCard = null; // 현재 확대된 카드

    private void Start()
    {
        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        originalScale = transform.localScale;
        originalSiblingIndex = transform.GetSiblingIndex();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (currentlyZoomedCard == null || currentlyZoomedCard == this)
            {
                ZoomIn();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (currentlyZoomedCard == this)
            {
                ZoomOut();
            }
        }
    }

    public void ZoomIn()
    {
        if (isZoomed || (currentlyZoomedCard != null && currentlyZoomedCard != this)) return;

        transform.localScale = originalScale * zoomScale;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.SetAsLastSibling(); // 맨 위로 올리기

        isZoomed = true;
        currentlyZoomedCard = this; // 현재 확대된 카드로 설정
    }

    public void ZoomOut()
    {
        if (!isZoomed) return;

        transform.localScale = originalScale;
        transform.rotation = originalRotation;
        transform.SetSiblingIndex(originalSiblingIndex); // 초기 순서로 되돌리기

        isZoomed = false;
        currentlyZoomedCard = null; // 현재 확대된 카드 해제
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
