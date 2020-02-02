using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARWorldObjectControl : MyComponent
{
    [SerializeField]
    protected string entranceKey;

    [SerializeField]
    protected Material[] materials;

    [SerializeField]
    protected bool isTigerObject = false;

    protected 

    bool isShowing = false;


    //float hideEntranceRatio = 0.2f;
    float hideEntranceRatio = 1.0f;

    protected override void _set(Dictionary<string, object> args = null)
    {
        base._set(args);

        MeshRenderer[] meshRends = this.GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] skinMeshRends = this.GetComponentsInChildren<SkinnedMeshRenderer>();

        materials = new Material[meshRends.Length + skinMeshRends.Length];

        for(int i=0; i<meshRends.Length; i++)
        {
            materials[i] = meshRends[i].materials[0];
        }
        for (int i = 0; i < skinMeshRends.Length; i++)
        {
            materials[meshRends.Length + i] = skinMeshRends[i].materials[0];
        }

        entranceRatio(hideEntranceRatio);
    }

    protected virtual void entranceRatio(float value)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i] != null)
            {
                Color matColor = materials[i].color;
                matColor.a = value;
                materials[i].color = matColor;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(entranceKey))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                isShowing = false;
                LeanTween.value(gameObject, (_ratio) =>
                {
                    this.entranceRatio(_ratio);
                }, 1.0f, hideEntranceRatio, 1.0f);
            }
            else
            {
                isShowing = true;
                LeanTween.value(gameObject, (_ratio) =>
                {
                    this.entranceRatio(_ratio);
                }, hideEntranceRatio, 1.0f, 1.0f);
            }
        }

        if (isTigerObject)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {

            }
        }
    }
}
