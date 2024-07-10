using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    public float zoomScale = 1.5f; // Ȯ�� ����
    public float zoomDuration = 0.25f; // ��� �ִϸ��̼� ���� �ð�

    private Vector3 originalScale; // ���� ũ��
    private Vector3 originalPosition; // ���� ��ġ
    private Quaternion originalRotation; // ���� ȸ��
    private int originalSiblingIndex; // �ʱ� ���� ���� ����
    private bool isZoomed = false; // Ȯ�� ���� üũ

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
            transform.SetAsLastSibling(); // �� ���� �ø���
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
        transform.SetSiblingIndex(originalSiblingIndex); // �ʱ� ������ �ǵ�����

        isZoomed = false;
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
