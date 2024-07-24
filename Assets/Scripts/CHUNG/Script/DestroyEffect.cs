using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public float destroyDelay=4f;
    void Start()
    {
        StartCoroutine(DestroyEffectDelay());
    }

    IEnumerator DestroyEffectDelay()
    {
        yield return new WaitForSecondsRealtime(destroyDelay);
        Destroy(this.gameObject);
    }
}
