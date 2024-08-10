using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardOutline : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private float animationDuration = 1f; // �ִϸ��̼� ���� �ð�
    [SerializeField] CardBasic cardBasic;
    private Material cardMaterial;

    protected void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            cardImage.gameObject.SetActive(true);
        }

        if (cardImage != null)
        {
            cardMaterial = cardImage.material;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 3) return;

        if (cardImage != null && GameManager.instance.player != null && GameManager.instance.playerTurn)
        {
            // �÷��̾��� ����� ī�� ��� �̻��� ��� �̹��� Ȱ��ȭ, �׷��� ������ ��Ȱ��ȭ
            cardImage.enabled = GameManager.instance.player.currentCost >= cardBasic.cost;

            if (cardImage.enabled)
            {
                StartCoroutine(AnimateOutlineAlpha());
            }
        }
    }

    private IEnumerator AnimateOutlineAlpha()
    {
        if (cardMaterial == null) yield break;

        Color outlineColor = cardMaterial.GetColor("_OutlineColor");
        float elapsedTime = 0f;

        // ���� ���� 1���� 0���� �ε巴�� ��ȭ�ϵ��� �ִϸ��̼�
        while (elapsedTime < animationDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / animationDuration);
            outlineColor.a = alpha;
            cardMaterial.SetColor("_OutlineColor", outlineColor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �ִϸ��̼� ���� �� ���� ���� 0���� ����
        outlineColor.a = 0f;
        cardMaterial.SetColor("_OutlineColor", outlineColor);
    }
}
