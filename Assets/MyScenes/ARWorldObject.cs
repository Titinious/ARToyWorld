using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ARWorldObject : MyComponent
{
    ImageTargetBehaviour myImageTargetBehaviour;
    public ImageTargetBehaviour MyImageTargetBehavious
    {
        get
        {
            return myImageTargetBehaviour;
        }
    }
    Pose originalLocalPose;

    int notDetectCnt = 0;

    [SerializeField]
    bool exemptPlacement = false;

    protected override void _set(Dictionary<string, object> args = null)
    {
        originalLocalPose.position = transform.localPosition;
        originalLocalPose.rotation = transform.localRotation;

        myImageTargetBehaviour = this.GetComponentInParent<ImageTargetBehaviour>();

        base._set(args);
    }
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HabitariumTerrain.Instance != null)
        {

            if (HabitariumTerrain.Instance.lockMode)
            {
                HabitariumTerrain.LockARWorldObject(this);
                return;
            }
            else
            {
                HabitariumTerrain.UnlockARWorldObject(this);
            }
            
        }

        if (HabitariumTerrain.Instance.placeMode && !exemptPlacement)
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << LayerMask.NameToLayer("Terrain");

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            //layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position + transform.up * 10, -transform.up, out hit, Mathf.Infinity, layerMask))
            {
                notDetectCnt = 0;
                //Debug.DrawRay(transform.position + transform.up * 10, -transform.up * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
                //Debug.Log(hit.point);
                transform.position = hit.point;
            }
            else
            {
                notDetectCnt++;
                if (notDetectCnt >= 16)
                {
                    transform.localPosition = originalLocalPose.position;
                    transform.localRotation = originalLocalPose.rotation;
                }
                //Debug.DrawRay(transform.position, -transform.up * 1000, Color.white);
                //Debug.Log("Did not Hit");

            }
        }
        else
        {
            transform.localPosition = originalLocalPose.position;
            transform.localRotation = originalLocalPose.rotation;
        }

        
    }
}
