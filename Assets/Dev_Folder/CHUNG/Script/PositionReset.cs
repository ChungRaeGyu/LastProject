using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour
{
    RectTransform rectTransform;
    Vector2 initTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initTransform = rectTransform.position;
    }

    private void OnEnable()
    {
        rectTransform.position = initTransform;
    }
}
