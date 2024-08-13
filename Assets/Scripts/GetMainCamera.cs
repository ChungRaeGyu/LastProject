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
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if (canvas == null)
        {
            return;
        }
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 0.4f;
    }
}
