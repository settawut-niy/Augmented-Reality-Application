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

    // Control swaping
    [SerializeField] Toggle toggle_AutoSwap;
    [SerializeField] Toggle toggle_ConfirmSwap;
    [SerializeField] Text text_CountingSwap;
    bool isConfirmSwap = false;
    bool isAutoSwap = true;

    // UI setting Control
    [SerializeField] Button button_SettingControl;
    [SerializeField] RectTransform ui_Setting;

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

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += TrackedImageChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= TrackedImageChanged;
    }

    void TrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
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
            // I change from trackedImage.name to trackedImage.referenceImage.name
            collectedARObjects[trackedImage.referenceImage.name].SetActive(false);
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
            if (isAutoSwap)
            {
                toggle_ConfirmSwap.GetComponentInChildren<Text>().text = "     Swap Toggle: Auto";
                if (!toggle_ConfirmSwap.isOn)
                StartCoroutine(AutoSwap());
            }
        }
        else
        {
            toggle_ConfirmSwap.isOn = false;
        }
    }

    IEnumerator AutoSwap()
    {
        isAutoSwap = false;
        ChangeCountingText("2");
        yield return new WaitForSeconds(1);
        ChangeCountingText("1");
        yield return new WaitForSeconds(1);
        toggle_ConfirmSwap.isOn = true;
        ChangeCountingText("Swap!");
        yield return new WaitForSeconds(1);
        ChangeCountingText("");
    }

    void ChangeCountingText(string text)
    {
        if (text != null)
        {
            if (!text_CountingSwap.enabled)
            {
                text_CountingSwap.enabled = true;
            }
            text_CountingSwap.text = text;
        }
        else
        {
            text_CountingSwap.enabled = false;
        }
    }

    public void ToggleConfirmSwap()
    {
        if (!isConfirmSwap)
        {
            isConfirmSwap = true;
        }
        else
        {
            isConfirmSwap = false;

            if (toggle_AutoSwap.isOn)
            {
                isAutoSwap = true;
            }
        }
    }

    public void ToggleAutoSwap()
    {
        if (toggle_AutoSwap.isOn)
        {
            isAutoSwap = true;
            toggle_ConfirmSwap.interactable = false;
            toggle_ConfirmSwap.GetComponentInChildren<Text>().text = "     Swap Toggle: Auto";
        }
        else
        {
            isAutoSwap = false;
            toggle_ConfirmSwap.interactable = true;
            toggle_ConfirmSwap.GetComponentInChildren<Text>().text = "     Swap Toggle: Manual";
        }
    }

    public void UISettingControl()
    {
        if (ui_Setting.gameObject.activeSelf)
        {
            ui_Setting.gameObject.SetActive(false);
            button_SettingControl.GetComponentInChildren<Text>().text = "≡";
            button_SettingControl.GetComponent<RectTransform>().anchoredPosition = new Vector2(-25f, -25f);
        }
        else
        {
            ui_Setting.gameObject.SetActive(true);
            button_SettingControl.GetComponentInChildren<Text>().text = "▲";
            button_SettingControl.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -170f);
        }
    }
}
