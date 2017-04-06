using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Camera cameraMain;
    public Transform[] cameraPosition;
    int cameraPositionNumber, currentPosition = 0;

    void Start()
    {
        cameraPositionNumber = cameraPosition.Length-1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (currentPosition < cameraPositionNumber)
                currentPosition++;
            else
                currentPosition=0;
            cameraMain.transform.localPosition = cameraPosition[currentPosition].localPosition;
            cameraMain.transform.localRotation = cameraPosition[currentPosition].localRotation;
        }
    }
}
