using System;
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
public class ARMap : MonoBehaviour
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
        // Set canvas camera
        var canvas = trackedImage.GetComponentInChildren<Canvas>();
        canvas.worldCamera = worldSpaceCanvasCamera;

        Vector2 normalizedMapLocation = new Vector2(-9999, -9999);

        if (mapBottomLeftAnchor != null && mapTopRightAnchor != null)
        {
            Vector3 locationVector = trackedImage.transform.position - mapBottomLeftAnchor.position;

            float xLoc = Vector3.Dot(mapBottomLeftAnchor.right, locationVector);
            float yLoc = Vector3.Dot(mapBottomLeftAnchor.forward, locationVector);

            //Debug.Log("loc:" + xLoc + " " + yLoc);

            normalizedMapLocation = new Vector2(xLoc / mapWidth, yLoc / mapHeight);
        }

        // Update information about the tracked image
        var text = canvas.GetComponentInChildren<Text>();
        text.text = string.Format(
            "{0}\ntrackingState: {1}\nGUID: {2}\nReference size: {3} cm\nDetected size: {4} cm\n Normalized Map Location: {5}",
            trackedImage.referenceImage.name,
            trackedImage.trackingState,
            trackedImage.referenceImage.guid,
            trackedImage.referenceImage.size * 100f,
            trackedImage.size * 100f,
            normalizedMapLocation
        );

        var planeParentGo = trackedImage.transform.GetChild(0).gameObject;
        var planeGo = planeParentGo.transform.GetChild(0).gameObject;

        // Disable the visual plane if it is not being tracked
        if (trackedImage.trackingState != TrackingState.None)
        {
            planeGo.SetActive(true);

            // The image extents is only valid when the image is being tracked
            trackedImage.transform.localScale = new Vector3(trackedImage.size.x, 1f, trackedImage.size.y);

            // Set the texture
            var material = planeGo.GetComponentInChildren<MeshRenderer>().material;
            material.mainTexture = (trackedImage.referenceImage.texture == null) ? defaultTexture : trackedImage.referenceImage.texture;
        }
        else
        {
            planeGo.SetActive(false);
        }
    }

    [SerializeField]
    public Transform mapBottomLeftAnchor = null;
    [SerializeField]
    public Transform mapTopRightAnchor = null;
    [SerializeField]
    public Transform testLocation = null;

    float mapWidth = 0;
    float mapHeight = 0;

    //private void Update()
    //{
    //    if (mapBottomLeftAnchor != null && mapTopRightAnchor != null)
    //    {
    //        Vector3 mapVector = mapTopRightAnchor.position - mapBottomLeftAnchor.position;

    //        float width = Vector3.Dot(mapBottomLeftAnchor.right, mapVector);
    //        float height = Vector3.Dot(mapBottomLeftAnchor.forward, mapVector);

    //        //Debug.Log("Map:" + width + " " + height);

    //        Vector3 locationVector = testLocation.position - mapBottomLeftAnchor.position;

    //        float xLoc = Vector3.Dot(mapBottomLeftAnchor.right, locationVector);
    //        float yLoc = Vector3.Dot(mapBottomLeftAnchor.forward, locationVector);

    //        //Debug.Log("loc:" + xLoc + " " + yLoc);

    //        Vector2 norLoc = new Vector2(xLoc / width, yLoc / height);
    //        Debug.Log(norLoc);
    //    }

    //}

    void UpdateMapAnchor(ARTrackedImage trackedImage){

        if(trackedImage.referenceImage.name == "A"){
            mapBottomLeftAnchor = trackedImage.transform;
        }
        else if(trackedImage.referenceImage.name == "B")
        {
            mapTopRightAnchor = trackedImage.transform;
        }


        if (mapBottomLeftAnchor != null && mapTopRightAnchor != null)
        {
            Vector3 mapVector = mapTopRightAnchor.position - mapBottomLeftAnchor.position;

            mapWidth = Vector3.Dot(mapBottomLeftAnchor.right, mapVector);
            mapHeight = Vector3.Dot(mapBottomLeftAnchor.forward, mapVector);
        }

    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            // Give the initial image a reasonable default scale
            trackedImage.transform.localScale = new Vector3(0.01f, 1f, 0.01f);

            UpdateMapAnchor(trackedImage);
            UpdateInfo(trackedImage);

        }

        foreach (var trackedImage in eventArgs.updated)
            UpdateInfo(trackedImage);
    }
}
