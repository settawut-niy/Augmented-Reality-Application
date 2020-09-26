using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCharacter : MonoBehaviour
{
    [SerializeField] SphereCharacter otherSphereCharacter;

    Renderer sphereRenderer;
    Color randomColor;

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();

        GenerateRandomColor();
    }

    void Update()
    {
        CheckSameColor();
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
