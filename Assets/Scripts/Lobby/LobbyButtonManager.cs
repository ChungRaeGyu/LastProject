using System.Collections;
using System.Collections.Generic;
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
                // 연속 클릭 방지
                if (isRestoringColor) return;

                // 텍스트 색 빨간색
                Color originalColor = LobbyManager.instance.currentCrystal.color; // 원래 색상 저장
                LobbyManager.instance.currentCrystal.color = Color.red;

                // TextBlink를 활성화하여 깜빡이게 하기
                TextBlink blinkScript = LobbyManager.instance.currentCrystal.GetComponent<TextBlink>();
                if (blinkScript != null)
                {
                    blinkScript.enabled = true; // 스크립트 활성화
                }
                else
                {
                    blinkScript = LobbyManager.instance.currentCrystal.gameObject.AddComponent<TextBlink>();
                    blinkScript.blinkDuration = 0.2f; // 원하는 깜빡임 주기 설정
                }

                // 깜빡임 후 색상을 원래대로 되돌리기
                StartCoroutine(RestoreOriginalColorAfterBlink(blinkScript, originalColor));

                AudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);
            }
        }
    }

    private IEnumerator RestoreOriginalColorAfterBlink(TextBlink blinkScript, Color originalColor)
    {
        isRestoringColor = true;

        // 깜빡임을 얼마동안 할지를 설정
        yield return new WaitForSeconds(0.5f);

        // TextBlink 스크립트 비활성화
        if (blinkScript != null)
        {
            blinkScript.enabled = false;
        }

        // 원래 색으로 복원
        LobbyManager.instance.currentCrystal.color = originalColor;

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
        AudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);
        DataManager.Instance.SuffleDeckList();
        SceneManager.LoadScene(2);
    }
}
