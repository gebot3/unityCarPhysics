using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLights : MonoBehaviour {

    Car car;
    public Material lightsOnMaterial,lightsOffMaterial,brakeLightMaterial;
    Renderer render;
    public GameObject[] headLights,otherLights,brakeLights;

    bool brakeLightsOn;

    void Awake()
    {
        car = GetComponent<Car>();
    }

    public void TurnLights(GameObject[] theLights, bool on, int start)
    {

    }

    public void braking()
    {
        brakeLightsOn = true;
        for (int i = 0; i < brakeLights.Length; i++)
        {
            render = brakeLights[i].GetComponent<Renderer>();
            if (render.material!=brakeLightMaterial)
                render.material = brakeLightMaterial;
        }
    }

    public void notBraking()
    {
        Material defaultMat;
        if (car.lightsOn)
        {
            defaultMat = lightsOnMaterial;
        }
        else
            defaultMat = lightsOffMaterial;
        for (int i = 0; i < brakeLights.Length; i++)
        {
            render = brakeLights[i].GetComponent<Renderer>();
            if (render.material != defaultMat)
            {
                render.material = defaultMat;
            }
        }
    }
    
    void Update()
    {
        /*if (car.braking)
        {
            braking();
        }
        else
        {
            if (brakeLightsOn)
            {
                notBraking();
            }
        }*/
        if (Input.GetKeyDown(KeyCode.DownArrow))
            braking();
        if (Input.GetKeyUp(KeyCode.DownArrow))
            notBraking();
    }
}
