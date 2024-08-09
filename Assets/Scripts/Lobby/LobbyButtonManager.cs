using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyButtonManager : MonoBehaviour
{
    [Header("OpenCanvas")]
    [SerializeField] GameObject BookCanvas;
    [SerializeField] GameObject DeckCanvas;
    [SerializeField] GameObject DrawCanvas;

    [Header("Sound")]
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip GachaClip;

    private DrawSystem drawSystem;

    private bool isRestoringColor = false; // 색을 복원중인 것을 나타냄

    private void Start()
    {
        drawSystem = DrawCanvas.GetComponentInParent<DrawSystem>();
    }
    public void ControlBookCanvas()
    {
        BookCanvas.SetActive(!BookCanvas.activeInHierarchy);
        DeckCanvas.SetActive(!DeckCanvas.activeInHierarchy);
    }

    #region DrawSystem
    public void ControlDrawCanvas()
    {
        if (DrawCanvas.activeInHierarchy)
        {
            AudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);
            DrawCanvas.SetActive(false);
            drawSystem.CloseCanvas();
        }
        else
        {
            if (DataManager.Instance.currentCrystal >= 500)
            {
                DataManager.Instance.currentCrystal -= 500;
                LobbyManager.instance.currentCrystal.text = DataManager.Instance.currentCrystal.ToString(); // 나중에 메서드로 정리해서 호출해야함 (임시)

                AudioSource.PlayOneShot(GachaClip);
                DrawCanvas.SetActive(true);
                GaChaBtn();
            }
            else
            {
                BlinkText(LobbyManager.instance.currentCrystal, Color.red, 0.5f, 0.2f);
                SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);
            }
        }
    }

    public void BlinkText(TMP_Text textComponent, Color blinkColor, float blinkDuration, float blinkInterval)
    {
        // 연속 클릭 방지
        if (isRestoringColor) return;

        StartCoroutine(BlinkTextCoroutine(textComponent, blinkColor, blinkDuration, blinkInterval));
    }

    private IEnumerator BlinkTextCoroutine(TMP_Text textComponent, Color blinkColor, float blinkDuration, float blinkInterval)
    {
        isRestoringColor = true;

        Color originalColor = textComponent.color;
        TextBlink blinkScript = textComponent.GetComponent<TextBlink>();
        if (blinkScript == null)
        {
            blinkScript = textComponent.gameObject.AddComponent<TextBlink>();
        }
        blinkScript.blinkDuration = blinkInterval; // 깜빡임 주기 설정
        blinkScript.enabled = true;

        textComponent.color = blinkColor;

        yield return new WaitForSeconds(blinkDuration);

        blinkScript.enabled = false;
        textComponent.color = originalColor;

        isRestoringColor = false;
    }

    public void GaChaBtn()
    {
        drawSystem.DrawingCardBtn();
    }
    public void OpenCardBtn()
    {
        drawSystem.OpenCard();
    }
    #endregion
    public void AddDeckBtn()
    {

    }
    public void GotoStageBoardBtn()
    {
        if (DataManager.Instance.LobbyDeck.Count < 6)
        {
            Debug.Log("카드가 부족해요~ 6장을 채워 주세요");
            return;
        }

        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);
        DataManager.Instance.SuffleDeckList();
        SceneFader.instance.LoadSceneWithFade(2);
    }
}
