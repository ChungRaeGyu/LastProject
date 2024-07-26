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

    private bool isRestoringColor = false; // ���� �������� ���� ��Ÿ��

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
                LobbyManager.instance.currentCrystal.text = DataManager.Instance.currentCrystal.ToString(); // ���߿� �޼���� �����ؼ� ȣ���ؾ��� (�ӽ�)

                AudioSource.PlayOneShot(GachaClip);
                DrawCanvas.SetActive(true);
                GaChaBtn();
            }
            else
            {
                // ���� Ŭ�� ����
                if (isRestoringColor) return;

                // �ؽ�Ʈ �� ������
                Color originalColor = LobbyManager.instance.currentCrystal.color; // ���� ���� ����
                LobbyManager.instance.currentCrystal.color = Color.red;

                // TextBlink�� Ȱ��ȭ�Ͽ� �����̰� �ϱ�
                TextBlink blinkScript = LobbyManager.instance.currentCrystal.GetComponent<TextBlink>();
                if (blinkScript != null)
                {
                    blinkScript.enabled = true; // ��ũ��Ʈ Ȱ��ȭ
                }
                else
                {
                    blinkScript = LobbyManager.instance.currentCrystal.gameObject.AddComponent<TextBlink>();
                    blinkScript.blinkDuration = 0.2f; // ���ϴ� ������ �ֱ� ����
                }

                // ������ �� ������ ������� �ǵ�����
                StartCoroutine(RestoreOriginalColorAfterBlink(blinkScript, originalColor));

                AudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);
            }
        }
    }

    private IEnumerator RestoreOriginalColorAfterBlink(TextBlink blinkScript, Color originalColor)
    {
        isRestoringColor = true;

        // �������� �󸶵��� ������ ����
        yield return new WaitForSeconds(0.5f);

        // TextBlink ��ũ��Ʈ ��Ȱ��ȭ
        if (blinkScript != null)
        {
            blinkScript.enabled = false;
        }

        // ���� ������ ����
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
