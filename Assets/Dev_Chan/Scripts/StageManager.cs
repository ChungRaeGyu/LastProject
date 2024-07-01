using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public GameObject[] stage = new GameObject[6];
    public GameObject clear;
    public GameObject player;


    public void Start()
    {
        player.transform.position = new Vector3(-6, 1, 0);

    }

    public void FixedUpdate()
    {

    }
}
