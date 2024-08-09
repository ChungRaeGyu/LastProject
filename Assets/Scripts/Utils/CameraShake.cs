using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    public float ShakeAmount;

    public float ShakeTime;
    Vector3 initialPosition;


    // Update is called once per frame

    public void VirbraterForTime(float time)
    {
        initialPosition = transform.position;
        ShakeTime = time;
    }

    /*
    public void Update()
    {
        if (ShakeTime > 0)
        {
            Debug.Log("실행");
            transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            transform.position = initialPosition;
        }
    }*/
    public void Coroutine()
    {
        StartCoroutine(ShakeCamera());
    }
    IEnumerator ShakeCamera()
    {
        initialPosition = transform.position;
        ShakeTime = 0.2f;
        while (true)
        {
            if (ShakeTime > 0)
            {
                Debug.Log("실행");
                transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
                ShakeTime -= Time.deltaTime;
                yield return null;
            }
            else
            {
                ShakeTime = 0.0f;
                transform.position = initialPosition;
                break;
            }
        }
    }
}
