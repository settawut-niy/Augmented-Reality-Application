using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageOperator : MonoBehaviour
{
    ARTrackedImageManager trackedImageManager;

    // Object that want to spawn
    [SerializeField] GameObject[] selectedObjectPrefabs;

    // Place that keep object ready to use (spawn)
    Dictionary<string, GameObject> collectedARObjects = new Dictionary<string, GameObject>();

    // Temporary : For Test - todo delete
    [SerializeField] Text debugText;
    int d_onEnable;
    int d_onDisable;
    int d_added;
    int d_updated;
    int d_removed;
    string d_name;
    bool d_active;

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach (GameObject objectPrefab in selectedObjectPrefabs)
        {
            GameObject newObjectPrefab = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
            newObjectPrefab.name = objectPrefab.name;
            collectedARObjects.Add(objectPrefab.name, newObjectPrefab);
            collectedARObjects[objectPrefab.name].SetActive(false);
        }
    }

    // Temporary : For Test - todo delete
    void Update()
    {
        debugText.text =
            "OnEnable: " + d_onEnable + "\n" +
            "OnDisable: " + d_onDisable + "\n" +
            "Added: " + d_added + "\n" +
            "Updated: " + d_updated + "\n"+
            "Removed: " + d_removed + "\n" +
            "Tracked Active: " + d_name + " " + d_active
            ;
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += TrackedImageChanged;

        // todo delete
        d_onEnable++;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= TrackedImageChanged;

        // todo delete
        d_onDisable++;
    }

    void TrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);

            // todo delete
            d_added++;
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);

            // todo delete
            d_updated++;
            d_name = trackedImage.referenceImage.name;
            d_active = trackedImage.gameObject.activeInHierarchy;
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            // todo it have to fix!
            collectedARObjects[trackedImage.name].SetActive(false);

            // todo delete
            d_removed++;
        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState != TrackingState.None)
        {
            SpawnARObject(trackedImage.referenceImage.name, trackedImage.transform.position);
        }
        else 
        {
            DespawnARObject(trackedImage.referenceImage.name);
        }
    }

    void SpawnARObject(string name, Vector3 position)
    {
        collectedARObjects[name].SetActive(true);
        collectedARObjects[name].GetComponent<SphereCharacter>().OwnTrackedImagePosition.Set(position.x, position.y, position.z);
        collectedARObjects[name].GetComponent<SphereCharacter>().PositionBehavior();
    }

    void DespawnARObject(string name)
    {
        collectedARObjects[name].SetActive(false);
    }
}
