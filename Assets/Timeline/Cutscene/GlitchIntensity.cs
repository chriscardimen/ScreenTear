using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using URPGlitch.Runtime.AnalogGlitch;
using UnityEngine.SceneManagement;

public class GlitchIntensity : MonoBehaviour
{
    [SerializeField] Volume volume;
    AnalogGlitchVolume analogGlitchVolume;
    public float whenToInvoke = 0f;
    public float whenToCancelEffect = 0f;

    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            volume.profile.TryGet<AnalogGlitchVolume>(out analogGlitchVolume);
            Invoke("ChangeEffect", whenToInvoke);
            Invoke("ResetEffect", whenToCancelEffect);
            Invoke("ChangeEffect", 4.0f);
            Invoke("ResetEffect", 5.0f);
            Invoke("ChangeEffect", 10.7f);
            Invoke("ResetEffect", 11.2f);
            Invoke("ChangeEffect", 15.6f);
            Invoke("ResetEffect", 16.1f);
            Invoke("ChangeEffect", 18.0f);
            Invoke("ResetEffect", 18.5f);
            Invoke("ChangeEffect", 19.5f);
            Invoke("ResetEffect", 20.0f);
            Invoke("ChangeEffect", 20.7f);
            Invoke("ResetEffect", 21.2f);
            Invoke("ChangeEffect", 26.4f);
            Invoke("ResetEffect", 27.4f);
        }


    }

    public void ChangeEffect()
    {
        analogGlitchVolume.scanLineJitter.value = 0.75f;
        analogGlitchVolume.verticalJump.value = 0.1f;
        analogGlitchVolume.horizontalShake.value = 0.2f;
        analogGlitchVolume.colorDrift.value = 0.4f;
    }

    public void ResetEffect()
    {
        analogGlitchVolume.scanLineJitter.value = 0f;
        analogGlitchVolume.verticalJump.value = 0f;
        analogGlitchVolume.horizontalShake.value = 0f;
        analogGlitchVolume.colorDrift.value = 0f;
    }
}
