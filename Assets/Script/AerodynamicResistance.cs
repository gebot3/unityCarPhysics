﻿using UnityEngine;
using System;
using System.Collections;

public class AerodynamicResistance : MonoBehaviour {

    public Vector3 coefficient;
    public Vector3 magnitude, force;


	void FixedUpdate()
    {
        magnitude = gameObject.GetComponent<Rigidbody>().velocity;
        force = Vector3.Scale(Vector3.Scale(magnitude, magnitude), -2 * coefficient);
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(force);
    }
}