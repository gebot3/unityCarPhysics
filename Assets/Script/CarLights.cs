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
        Debug.Log("BRAKING");
        brakeLightsOn = true;
        for (int i = 0; i < brakeLights.Length; i++)
        {
            brakeLights[i].SetActive(true);
        }
    }

    public void notBraking()
    {
        for (int i = 0; i < brakeLights.Length; i++)
        {
            brakeLights[i].SetActive(false);
        }
    }
}
