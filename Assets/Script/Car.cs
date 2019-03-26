using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
    [Header("Car Settings")]
    private Rigidbody rB;
    public float wheelRadius;
    public float steeringAngle;
    public float maxSteeringAngle;
    public float throttle, throttleAssistTimer;
    public bool  throttleAssist;
    public Transform centreGLocation;
    public float maxSpeed;

    [Header("Transmission")]
    public float[] gearRatio;

    [Header("Wheels")]
    public float brakeTorque, handBrakeTorque;
    public bool braking;
    public GameObject[] fWheels;
    public GameObject[] fWheelsmesh;
    public GameObject[] rWheels;
    public GameObject[] rWheelsmesh;

    [Header("CarProperties")]
    public bool lightsOn;

    void Awake()
    {
        rB = GetComponent<Rigidbody>();
        rB.centerOfMass = new Vector3(centreGLocation.localPosition.x* transform.localScale.x, centreGLocation.localPosition.y* transform.localScale.y, centreGLocation.localPosition.z* transform.localScale.z);
    }

    public Vector3 Centre
    {
        get{
            return rB.centerOfMass;
        }   
    }
    //PhysicsFunction

    public Rigidbody getRigidbody() {
        return rB;
    }

    void Update()
    {
        steeringAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
        turning(steeringAngle);
        if (!throttleAssist)
            throttle = Input.GetAxis("Vertical");
        if (Input.GetAxis("Vertical") < 0.0f)
            braking = true;
        else
            if (braking)
            braking = false;
        rotateWheels();
    }

    void turning(float _steeringAngle)
    {
        float carSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        if (_steeringAngle < 0)
        {
            steeringAngle = steeringAngle + (carSpeed / 11);
            if (steeringAngle > 0)
                steeringAngle = 0.0f;
        }
        else
        {
            steeringAngle = steeringAngle - (carSpeed / 2.0f);
            if (steeringAngle < 0)
                steeringAngle = 0.0f;
        }
        for (int i = 0; i < fWheels.Length; i++)
        {
            fWheels[i].GetComponent<WheelCollider>().steerAngle = _steeringAngle;
            float x,y,z;
            x = fWheelsmesh[i].transform.localEulerAngles.x;
            z = fWheelsmesh[i].transform.localEulerAngles.z;
            y = fWheels[i].GetComponent<WheelCollider>().steerAngle - z;
            fWheelsmesh[i].transform.localEulerAngles = new Vector3(x,y,z);
        }
    }

    //Audio/VisualFunction
    void rotateWheels()
    {
        for (int i = 0; i < rWheels.Length; i++)
        {
            rWheelsmesh[i].transform.Rotate(rWheels[i].GetComponent<WheelCollider>().rpm * 60 * Time.fixedDeltaTime , 0.0f, 0.0f);
            fWheelsmesh[i].transform.Rotate(fWheels[i].GetComponent<WheelCollider>().rpm * 60 * Time.fixedDeltaTime, 0.0f, 0.0f);
        }
    }
}
