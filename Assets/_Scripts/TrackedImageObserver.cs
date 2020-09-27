using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageObserver : MonoBehaviour
{
    ARTrackedImageManager trackedImageManager;
    Vector3 m_trackedImagePosition;

    public Vector3 TrackedImagePosition
    {
        get { return m_trackedImagePosition; }
    }

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {

        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        m_trackedImagePosition = trackedImage.transform.position;
    }
}
