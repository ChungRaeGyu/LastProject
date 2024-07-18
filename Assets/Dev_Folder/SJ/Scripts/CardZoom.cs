using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float zoomScale = 1.5f; // Ȯ�� ����
    public float zoomDuration = 0.25f; // ��� �ִϸ��̼� ���� �ð�

    private Vector3 originalScale; // ���� ũ��
    private Vector3 originalPosition; // ���� ��ġ
    private Quaternion originalRotation; // ���� ȸ��
    private int originalSiblingIndex; // �ʱ� ���� ���� ����
    private bool isZoomed = false; // Ȯ�� ���� üũ

    private static CardZoom currentlyZoomedCard = null; // ���� Ȯ��� ī��

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
        transform.SetAsLastSibling(); // �� ���� �ø���

        isZoomed = true;
        currentlyZoomedCard = this; // ���� Ȯ��� ī��� ����
    }

    public void ZoomOut()
    {
        if (!isZoomed) return;

        transform.localScale = originalScale;
        transform.rotation = originalRotation;
        transform.SetSiblingIndex(originalSiblingIndex); // �ʱ� ������ �ǵ�����

        isZoomed = false;
        currentlyZoomedCard = null; // ���� Ȯ��� ī�� ����
    }

    public void SetOriginalPosition(Vector3 position, Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
    }

    public void ResetPosition()
    {
        transform.rotation = originalRotation; // ī�� ȸ�� �ʱ�ȭ
    }
}
