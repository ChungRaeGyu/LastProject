using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetMainCamera : MonoBehaviour
{
    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        Debug.Log("����");    
        canvas.worldCamera = Camera.main;
    }
}
