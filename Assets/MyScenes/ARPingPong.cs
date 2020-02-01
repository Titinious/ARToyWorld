﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

/// This component listens for images detected by the <c>XRImageTrackingSubsystem</c>
/// and overlays some information as well as the source Texture2D on top of the
/// detected image.
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ARPingPong : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The camera to set on the world space UI canvas for each instantiated image info.")]
    Camera m_WorldSpaceCanvasCamera;

    /// <summary>
    /// The prefab has a world space UI canvas,
    /// which requires a camera to function properly.
    /// </summary>
    public Camera worldSpaceCanvasCamera
    {
        get { return m_WorldSpaceCanvasCamera; }
        set { m_WorldSpaceCanvasCamera = value; }
    }

    [SerializeField]
    [Tooltip("If an image is detected but no source texture can be found, this texture is used instead.")]
    Texture2D m_DefaultTexture;

    /// <summary>
    /// If an image is detected but no source texture can be found,
    /// this texture is used instead.
    /// </summary>
    public Texture2D defaultTexture
    {
        get { return m_DefaultTexture; }
        set { m_DefaultTexture = value; }
    }

    ARTrackedImageManager m_TrackedImageManager;

    PingPong pingpongBall;

    void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        pingpongBall = GameObject.Find("PingPong").GetComponent<PingPong>();
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }



    void UpdateInfo(ARTrackedImage trackedImage)
    {

        var gamePieceGo = trackedImage.gameObject;
        var pieces = gamePieceGo.transform.Find("Pieces");

        // Disable the visual plane if it is not being tracked
        if (trackedImage.trackingState != TrackingState.None)
        {
            pieces.gameObject.SetActive(true);

            //var imageInfoPanel = gamePieceGo.transform.Find("MyImageInfoPanel");


            // The image extents is only valid when the image is being tracked
            //imageInfoPanel.transform.localScale = new Vector3(trackedImage.size.x, 1f, trackedImage.size.y);

            //pieces.transform.Find("Sphere").gameObject.SetActive(true);
            //pieces.transform.Find("Sphere").gameObject.SetActive(true);

            if (trackedImage.referenceImage.name == "jungle"){
                pieces.transform.Find("Sphere").gameObject.SetActive(false);
                pingpongBall.source = gamePieceGo;
            }
            else if (trackedImage.referenceImage.name == "jungle_2")
            {
                pieces.transform.Find("Cube").gameObject.SetActive(false);
                pingpongBall.target = gamePieceGo;
            }

            // Set the texture
            //var material = imageInfoPanel.GetComponentInChildren<MeshRenderer>().material;
            //material.mainTexture = (trackedImage.referenceImage.texture == null) ? defaultTexture : trackedImage.referenceImage.texture;
        }
        else
        {
            pieces.gameObject.SetActive(false);
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            // Give the initial image a reasonable default scale
            //trackedImage.transform.localScale = new Vector3(0.01f, 1f, 0.01f);

            UpdateInfo(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
            UpdateInfo(trackedImage);

        foreach (var trackedImage in eventArgs.removed){
            var gamePieceGo = trackedImage.gameObject;
            var pieces = gamePieceGo.transform.Find("Pieces");
            pieces.gameObject.SetActive(false);
        }
    }
}
