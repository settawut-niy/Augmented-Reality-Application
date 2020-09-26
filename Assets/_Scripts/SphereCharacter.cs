using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCharacter : MonoBehaviour
{
    //Other Charater
    SphereCharacter otherSphereCharacter;

    //Own attribute
    [SerializeField] [Range(0, 10)] float speed = 1f;
    Renderer sphereRenderer;
    Color randomColor;
    Vector3 originPosition;
    float distanceBetweenCharacter;
    bool isWait = false;

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();
        originPosition = transform.position;

        GenerateRandomColor();
    }

    void Update()
    {
        FindOtherSphereCharacter();

        CheckSameColor();

        SwappingPosition();
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

    void SwappingPosition()
    {
        distanceBetweenCharacter = Vector3.Distance(otherSphereCharacter.originPosition, transform.position);

        if (isWait) { return; }

        transform.position = Vector3.Lerp(transform.position, otherSphereCharacter.originPosition, Time.deltaTime * speed);

        if (distanceBetweenCharacter < 0.03f)
        {
            isWait = true;
            transform.position = otherSphereCharacter.originPosition;
            StartCoroutine(WaitForNewSwapping());
        }
    }

    IEnumerator WaitForNewSwapping()
    {
        yield return new WaitForSeconds(2f);
        originPosition = transform.position;
        isWait = false;
    }
}
