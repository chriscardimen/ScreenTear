using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnscaledTimeShader : MonoBehaviour
{
    public List<Material> materialList;
    // Start is called before the first frame update
    void Start()
    {
        //crtMaterial = 
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Material mat in materialList)
        {
            mat.SetFloat("_Unscaled_Time", Time.unscaledTime);
        }
        //Debug.Log(GraphicsSettings.currentRenderPipeline.name);
        //Debug.Log(GraphicsSettings.GetCustomShader(Biu.Forward))

    }
}
