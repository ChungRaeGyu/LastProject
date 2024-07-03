using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Transform hpBarPos;
    public Slider healthSlider;

    public void Initialized(int maxhealth, int currenthealth, Transform transform)
    {
        hpBarPos = transform;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxhealth;
            healthSlider.value = currenthealth;
        }
    }

    private void Update()
    {
        if (hpBarPos != null)
            transform.position = hpBarPos.position;
        else
            Destroy(gameObject);
    }

    public void ResetHealthSlider(int currenthealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currenthealth;
        }
    }
}
