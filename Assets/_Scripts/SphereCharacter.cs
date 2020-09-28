using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCharacter : MonoBehaviour
{
    //Other Sphere Character in scene
    SphereCharacter otherSphereCharacter;

    //About tracked image
    Vector3 m_OwnTrackedImagePosition;
    bool isSetTrackedImagePosition = false;

    //Own attribute
    Renderer sphereRenderer;
    Color randomColor;
    float distanceToTrackedImage;
    float movementSpeed = 1f;
    bool isWaitForSwapping = false;

    public Vector3 OwnTrackedImagePosition
    {
        get { return m_OwnTrackedImagePosition; }
        set { m_OwnTrackedImagePosition = value; }
    }

    void Awake()
    {
        sphereRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        GenerateRandomColor();
    }

    void OnEnable()
    {
        isSetTrackedImagePosition = false;
        isWaitForSwapping = false;
    }

    void Update()
    {
        FindOtherSphereCharacter();

        CheckSameColor();
    }

    void FindOtherSphereCharacter()
    {
        if (FindObjectsOfType<SphereCharacter>().Length <= 1) { return; }

        if (otherSphereCharacter != null) { return; }

        for (int i = 0; i < FindObjectsOfType<SphereCharacter>().Length; i++)
        {
            if (otherSphereCharacter != null) { break; }

            if (this == FindObjectsOfType<SphereCharacter>()[i])
            {
                if (i == FindObjectsOfType<SphereCharacter>().Length - 1)
                {
                    otherSphereCharacter = FindObjectsOfType<SphereCharacter>()[0];
                }
                else
                {
                    otherSphereCharacter = FindObjectsOfType<SphereCharacter>()[i + 1];
                }
            }
        }

        print("I'm " + name + " and going to" + otherSphereCharacter.name);
    }

    void GenerateRandomColor()
    {
        randomColor = Random.ColorHSV();

        sphereRenderer.material.color = randomColor;
    }

    void CheckSameColor()
    {
        if (FindObjectsOfType<SphereCharacter>().Length <= 1) { return; }

        if (randomColor == otherSphereCharacter.randomColor)
        {
            GenerateRandomColor();
        }
    }

    public void PositionBehavior ()
    {
        SetPosition();

        SwappingPosition();
    }

    void SetPosition ()
    {
        if (isSetTrackedImagePosition) { return; }
        transform.position = m_OwnTrackedImagePosition;
        isSetTrackedImagePosition = true;
    }

    void SwappingPosition()
    {
        distanceToTrackedImage = Vector3.Distance(otherSphereCharacter.OwnTrackedImagePosition, transform.position);

        if (isWaitForSwapping) { return; }

        transform.position = Vector3.Lerp(transform.position, otherSphereCharacter.OwnTrackedImagePosition, Time.deltaTime * movementSpeed);

        if (distanceToTrackedImage < 0.03f)
        {
            isWaitForSwapping = true;
            transform.position = otherSphereCharacter.OwnTrackedImagePosition;
            StartCoroutine(WaitForNewSwapping());
        }
    }

    IEnumerator WaitForNewSwapping()
    {
        yield return new WaitForSeconds(2f);
        isSetTrackedImagePosition = false;
        isWaitForSwapping = false;
    }
}
