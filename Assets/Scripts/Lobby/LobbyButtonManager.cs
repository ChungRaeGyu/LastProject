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

    private void Start(){
        drawSystem = DrawCanvas.GetComponentInParent<DrawSystem>();
    }
    public void ControlBookCanvas(){
        BookCanvas.SetActive(!BookCanvas.activeInHierarchy);
        DeckCanvas.SetActive(!DeckCanvas.activeInHierarchy);
    }

    #region DrawSystem
    public void ControlDrawCanvas(){
        if (DrawCanvas.activeInHierarchy){
            AudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);
            DrawCanvas.SetActive(false);
            drawSystem.CloseCanvas();
        }else{
            AudioSource.PlayOneShot(GachaClip);
            DrawCanvas.SetActive(true);
            GaChaBtn();
        }
    }
    public void GaChaBtn(){
        drawSystem.DrawingCardBtn();
    }
    public void OpenCardBtn(){
        drawSystem.OpenCard();
    }
    #endregion
    public void AddDeckBtn(){
        
    }
    public void GotoStageBoardBtn(){
        if (DataManager.Instance.LobbyDeck.Count < 6)
        {
            Debug.Log("카드가 부족해요~ 6장을 채워 주세요");
            return;
        }
        
        AudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);
        foreach(var card in DataManager.Instance.LobbyDeck)
        {
            DataManager.Instance.deckList.Add(card);
        }
        DataManager.Instance.SuffleDeckList();
        SceneManager.LoadScene(2);
    }
}
