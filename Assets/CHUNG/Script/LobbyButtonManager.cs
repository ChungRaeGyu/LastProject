using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyButtonManager : MonoBehaviour
{
    [Header("BookPanel")]
    public GameObject BookPanel;

    public void ControlBookPanel(){
        BookPanel.SetActive(!BookPanel.activeInHierarchy);
    }
}
