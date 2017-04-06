using UnityEngine;
using System.Collections;

public class Suspension : MonoBehaviour {
    
    public Rigidbody rB;
    Car carScript;

    [Header("Suspension Settings")]
    public float springForce;
    public float damperForce;
    public float springConstant;
    public float damperConstant;
    public float restLength;

    [Header("Wheel Settings")]
    public GameObject wheel;

    private float previousLength, currentLength, springVelocity;
    
	void Start () {
        carScript = GetComponentInParent<Car>();
	}

	void FixedUpdate () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position,-transform.up,out hit,carScript.wheelRadius + restLength))
        {
            previousLength = currentLength;
            currentLength = restLength - (hit.distance - carScript.wheelRadius);
            springVelocity = (currentLength - previousLength) / Time.fixedDeltaTime;
            springForce = springConstant * currentLength;
            damperForce = damperConstant * springVelocity;
            Debug.DrawRay(transform.position, -transform.up, Color.red);
            rB.AddForceAtPosition(transform.up * (springForce + damperForce), transform.position);
            wheel.transform.position = hit.point + (carScript.wheelRadius * transform.up);
        }
        else
        {
            Debug.DrawRay(transform.position, -transform.up, Color.green);
            wheel.transform.position = transform.position - (restLength * transform.up);
        }
	}
    void LateUpdate()
    {

    }
}
