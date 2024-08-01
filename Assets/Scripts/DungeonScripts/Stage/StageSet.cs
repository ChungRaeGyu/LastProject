using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSet : MonoBehaviour
{
    //public GameObject outline;
    public GameObject entryBtn;

    public Vector3 entry1;
    public Vector3 entry2;
    public Vector3 entry3;

    private void Start()
    {
        entry1 = SaveManager.Instance.playerPosition + new Vector3(0.75f, 1.3f, 0.0f);
        entry2 = SaveManager.Instance.playerPosition + new Vector3(1.5f, 0.0f, 0.0f);
        entry3 = SaveManager.Instance.playerPosition + new Vector3(0.75f, -1.3f, 0.0f);

        if (this.gameObject.transform.position == entry1)
        {
            //outline.SetActive(true);
            entryBtn.SetActive(true);
        }
        else if (this.gameObject.transform.position == entry2)
        {
            //outline.SetActive(true);
            entryBtn.SetActive(true);
        }
        else if (this.gameObject.transform.position == entry3)
        {
            //outline.SetActive(true);
            entryBtn.SetActive(true);
        }
        else
        {
            //outline.SetActive(false);
            entryBtn.SetActive(false);
        }
    }
}
