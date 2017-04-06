using UnityEngine;
using System.Collections;

public class WheelR : MonoBehaviour {

    public Rigidbody rB;

    public Car carScript;
    public GameObject rearWheel;
    public float corneringStiffness;
    private float lateralForce;
    public float c;
    public Transform rearAxle;

    public void calculateForce()
    {
        lateralForce = CalculateSlipAngleRear();
    }

    void Start()
    {
        c = Vector3.Distance(rearAxle.localPosition, carScript.Centre);
    }

    public float getForce()
    {
        return lateralForce;
    }

    float LateralForce(float slipAngle)
    {
        float result;
        result = corneringStiffness * slipAngle;
        return result;
    }

    float CalculateSlipAngleRear()
    {
        float angularSpeed;
        float velocityLat, velocityLong;
        float result;
        angularSpeed = rB.angularVelocity.magnitude;
        velocityLong = rB.velocity.z;
        velocityLat = rB.velocity.x;
        result = Mathf.Atan((velocityLat + angularSpeed * c) / velocityLong);
        if (float.IsNaN(result)) return 0.0f;
        else
            return LateralForce(result);
    }
}
