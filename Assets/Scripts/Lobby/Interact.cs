using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Interact : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        LobbyManager.instance.currentCanvas = this.gameObject;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        LobbyManager.instance.currentCanvas = null;
    }
}
