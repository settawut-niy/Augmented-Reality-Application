using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCharacter : MonoBehaviour
{
    SphereCharacter otherSphereCharacter;

    Renderer sphereRenderer;
    Color randomColor;

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();

        GenerateRandomColor();
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
}
