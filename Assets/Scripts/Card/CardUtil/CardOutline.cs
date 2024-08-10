using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardOutline : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private float animationDuration = 1f; // 애니메이션 지속 시간
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
            // 플레이어의 비용이 카드 비용 이상일 경우 이미지 활성화, 그렇지 않으면 비활성화
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

        // 알파 값이 1에서 0으로 부드럽게 변화하도록 애니메이션
        while (elapsedTime < animationDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / animationDuration);
            outlineColor.a = alpha;
            cardMaterial.SetColor("_OutlineColor", outlineColor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 애니메이션 종료 후 알파 값을 0으로 설정
        outlineColor.a = 0f;
        cardMaterial.SetColor("_OutlineColor", outlineColor);
    }
}
