using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyButtonManager : MonoBehaviour
{
    [Header("OpenPanel")]
    public GameObject BookPanel;
    public GameObject DeckPanel;


    public void ControlBookPanel(){
        BookPanel.SetActive(!BookPanel.activeInHierarchy);
    }

    public void ControlDeckPanel(){
        DeckPanel.SetActive(!DeckPanel.activeInHierarchy);
    }
}
