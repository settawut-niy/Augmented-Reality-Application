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

    // Place that keep ready to use (spawn) object
    Dictionary<string, GameObject> collectedARObjects = new Dictionary<string, GameObject>();

    // Use to confirm swaping
    [SerializeField] Toggle toggle_ConfirmSwap;
    bool isConfirmSwap = false;
    bool hasObjectA = false;
    bool hasObjectB = false;

    // Temporary : For Test - todo delete <
    [SerializeField] Text debugText;
    int d_onEnable;
    int d_onDisable;
    int d_added;
    int d_updated;
    int d_removed;
    public string d_Info1;
    bool d_Info2;
    bool d_Info3;
    // Temporary : For Test - todo delete >

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

    // Temporary : For Test - todo delete <
    void Update()
    {
        debugText.text =
            "OnEnable: " + d_onEnable + "\n" +
            "OnDisable: " + d_onDisable + "\n" +
            "Added: " + d_added + "\n" +
            "Updated: " + d_updated + "\n"+
            "Removed: " + d_removed + "\n" +
            "Debug1: " + d_Info1 + "\n" +
            "Debug2: " + d_Info2 + "\n" +
            "Debug3: " + d_Info3
            ;
    }
    // Temporary : For Test - todo delete >

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += TrackedImageChanged;

        // todo delete <
        d_onEnable++;
        // todo delete >
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= TrackedImageChanged;

        // todo delete <
        d_onDisable++;
        // todo delete >
    }

    void TrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);

            // todo delete <
            d_added++;
            // todo delete >
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);

            // todo delete <
            d_updated++;
            // todo delete >
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            // I change from trackedImage.name to trackedImage.referenceImage.name
            collectedARObjects[trackedImage.referenceImage.name].SetActive(false);

            // todo delete <
            d_removed++;
            // todo delete >
        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            SpawnARObject(trackedImage.referenceImage.name, trackedImage.transform.position);
        }
        else 
        {
            DespawnARObject(trackedImage.referenceImage.name);
        }

        VerifyARObjectStable();
    }

    void SpawnARObject(string name, Vector3 position)
    {
        collectedARObjects[name].SetActive(true);
        if(!isConfirmSwap)
        {
            collectedARObjects[name].GetComponent<SphereCharacter>().OwnTrackedImagePosition = position;
        }
        collectedARObjects[name].GetComponent<SphereCharacter>().PositionBehavior(isConfirmSwap);
    }

    void DespawnARObject(string name)
    {
        collectedARObjects[name].SetActive(false);
    }

    void VerifyARObjectStable()
    {
        // selectedObjectPrefabs[0].name == "Sphere Object A"
        // selectedObjectPrefabs[1].name == "Sphere Object B"
        bool hasObjectA = collectedARObjects[selectedObjectPrefabs[0].name].activeSelf;
        bool hasObjectB = collectedARObjects[selectedObjectPrefabs[1].name].activeSelf;

        if (hasObjectA && hasObjectB)
        {
            toggle_ConfirmSwap.GetComponentInChildren<Text>().text = "Swap Now!";
            toggle_ConfirmSwap.interactable = true;
        }
        else
        {
            toggle_ConfirmSwap.GetComponentInChildren<Text>().text = "Can't Swap";
            toggle_ConfirmSwap.isOn = false;
            toggle_ConfirmSwap.interactable = false;
        }

        // todo delete <
        d_Info2 = hasObjectA;
        d_Info3 = hasObjectB;
        // todo delete >
    }

    public void ConfirmSwap ()
    {
        if (!isConfirmSwap)
        {
            isConfirmSwap = true;
        }
        else
        {
            isConfirmSwap = false;
        }
    }
}
