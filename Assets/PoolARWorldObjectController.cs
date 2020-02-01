using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolARWorldObjectController : ARWorldObjectControl
{
    [SerializeField]
    MeshRenderer poolWaterMeshRenderer;
    Material poolWaterMaterial;
    [SerializeField]
    float poolWaterOriginalTransparency = 0.314f;

    protected override void _set(Dictionary<string, object> args = null)
    {
        poolWaterMaterial = poolWaterMeshRenderer.materials[0];

        base._set(args);

    }

    protected override void entranceRatio(float value)
    {
        base.entranceRatio(value);

        poolWaterMaterial.SetFloat("_Transparency", value * poolWaterOriginalTransparency);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
