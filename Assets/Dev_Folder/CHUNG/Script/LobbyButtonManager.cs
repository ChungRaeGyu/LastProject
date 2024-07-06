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
        if(DrawCanvas.activeInHierarchy){
            DrawCanvas.SetActive(false);
            drawSystem.CloseCanvas();
        }else{
            DrawCanvas.SetActive(true);
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
        DataManager.Instance.SuffleDeckList();
        SceneManager.LoadScene(2);
    }
}
