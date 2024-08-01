using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class Test : MonoBehaviour
{

    public void OnClick()
    {
        EffekseerEffectAsset effect = Resources.Load<EffekseerEffectAsset>("Test");
        EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, transform.position);
        handle.SetRotation(transform.rotation);
    }
}
