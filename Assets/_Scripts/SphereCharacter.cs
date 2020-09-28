using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCharacter : MonoBehaviour
{
    // Temporary : For Test - todo delete
    TrackedImageOperator _TrackedImageOperator;

    //Other Sphere Character in scene
    SphereCharacter otherSphereCharacter;

    //About tracked image
    Vector3 m_OwnTrackedImagePosition;
    bool isSetTrackedImagePosition = false;

    //Own attribute
    Renderer sphereRenderer;
    Color[] colorPalette = { Color.white, Color.black, Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta };
    Color randomColor;
    float distanceToTrackedImage;
    float movementSpeed = 1f;
    public bool isSwapping = false;
    bool isWaitForSwapping = false;
    float count = 0;

    public Vector3 OwnTrackedImagePosition
    {
        get { return m_OwnTrackedImagePosition; }
        set { m_OwnTrackedImagePosition = value; }
    }

    void Awake()
    {
        sphereRenderer = GetComponent<Renderer>();
        // Temporary : For Test - todo delete
        _TrackedImageOperator = FindObjectOfType<TrackedImageOperator>();
    }

    void OnEnable()
    {
        GenerateRandomColor();
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
    }

    void GenerateRandomColor()
    {
        //randomedColor = Random.ColorHSV();
        randomColor = colorPalette[Random.Range(0, 7)];

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
        // Temporary : For Test - todo delete
        _TrackedImageOperator.d_Info1 = m_OwnTrackedImagePosition.ToString();

        // Temporary : For Test - todo delete
        _TrackedImageOperator.d_Info3 = count;

        SetPosition();

        SwappingPosition();
    }

    void SetPosition ()
    {
        //if (!otherSphereCharacter.gameObject.activeInHierarchy) { isSetTrackedImagePosition = false; }

        if (isSetTrackedImagePosition) { return; }

        transform.position = m_OwnTrackedImagePosition;

        if (transform.position == m_OwnTrackedImagePosition)
        {
            count += Time.deltaTime;
        }

        if (count > 2f)
        {
            isSetTrackedImagePosition = true;
            count = 0f;
        }
    }

    void SwappingPosition()
    {
        if (!otherSphereCharacter.gameObject.activeInHierarchy) { return; }

        isSwapping = true;

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
