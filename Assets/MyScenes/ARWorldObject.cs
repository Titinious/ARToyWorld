using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARWorldObject : MonoBehaviour
{
    Pose originalLocalPose;

    int notDetectCnt = 0;

    private void Awake()
    {
        originalLocalPose.position = transform.localPosition;
        originalLocalPose.rotation = transform.localRotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
            Debug.DrawRay(transform.position + transform.up * 10, -transform.up * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            Debug.Log(hit.point);
            transform.position = hit.point;
        }
        else
        {
            notDetectCnt++;
            if(notDetectCnt >= 32)
            {
                transform.localPosition = originalLocalPose.position;
                transform.localRotation = originalLocalPose.rotation;
            }
            Debug.DrawRay(transform.position, -transform.up * 1000, Color.white);
            //Debug.Log("Did not Hit");

        }
    }
}
