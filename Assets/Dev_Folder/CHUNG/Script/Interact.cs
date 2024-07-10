using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interact : MonoBehaviour, IPointerEnterHandler
{
    public GameObject currentInteract;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        currentInteract = this.gameObject;
    }
}
