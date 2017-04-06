using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text RPM, Gear;
    public Engine carEngine;
	
	// Update is called once per frame
	void Update () {
        RPM.text = "RPM : " + Mathf.Abs(carEngine.currentRPM);
        Gear.text = "Gear : " + carEngine.currentGear;
	}
}
