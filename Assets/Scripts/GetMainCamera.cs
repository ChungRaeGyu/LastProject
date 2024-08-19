using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetMainCamera : MonoBehaviour
{
    Canvas canvas;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        canvas = GetComponent<Canvas>();

        // Sorting Layer를 Setting으로 변경
        if (canvas != null)
        {
            canvas.sortingLayerName = "Setting";
        }
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if (canvas == null)
        {
            return;
        }

        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 0.4f;

        // Scene이 로드된 후 Sorting Layer를 Setting으로 변경
        canvas.sortingLayerName = "Setting";
    }
}
