using UnityEngine;
using System.Collections;

[System.Serializable]
public struct EngineData {
    public float maximumEngineTorque;
    public float minimumEngineTorque;
    public float minimumEngineTorqueAfterMaxRPM;
    public float maxTorqueRPM;
    public float redline;
}

public class Engine : MonoBehaviour {
    Car carData;
    public float engineTorque;
    public float wheelTorque;
    public EngineData engineData;
    public int currentGear;
    public float currentRPM;
    public float maxRPM;
    public float carSpeed,maxCarSpeed;
    public float driftTimer;
    public WheelCollider[] wheelsDrive;
    public WheelCollider[] steeringDrive;

    public bool automaticTransmission;

    public AudioSource engineSound, gearShiftSound;
    public float pitchModifier;

    void Awake()
    {
        carData = GetComponent<Car>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && currentGear <carData.gearRatio.Length-2)
        {
            if (!gearShiftSound.isPlaying)
                gearShiftSound.Play();
            carData.throttle = 0.0f;
            currentGear++;
        }

        if (Input.GetKeyDown(KeyCode.C) && currentGear>0)
        {
            if (!gearShiftSound.isPlaying)
                gearShiftSound.Play();
            carData.throttle = 0.0f;
            currentGear--;
        }
    }

    void Drift()
    {
        if (driftTimer > 0.0f)
        {
            driftTimer -= Time.fixedDeltaTime;
            if (driftTimer <= 0.0f)
            {
                WheelFrictionCurve wheelFriction = steeringDrive[0].sidewaysFriction;
                wheelFriction.stiffness = 1.5f;
                steeringDrive[0].sidewaysFriction = wheelFriction;
                wheelFriction = steeringDrive[1].sidewaysFriction;
                wheelFriction.stiffness = 1.5f;
                steeringDrive[1].sidewaysFriction = wheelFriction;
            }
        }
    }

    void FixedUpdate()
    {
        updateTorque();
        Acceleration();
        //Drift();
        if (automaticTransmission)
        {
            if (currentRPM > maxRPM - 500.0f && currentGear<5)
            {
                //if (!gearShiftSound.isPlaying)
                //    gearShiftSound.Play();
                carData.throttle = 0.0f;
                currentGear++;
            }
            if (currentRPM<3000.0f && currentGear>1)
            {   
                //if (!gearShiftSound.isPlaying)
                //    gearShiftSound.Play();
                carData.throttle = 0.5f;
                currentGear--;
            }
        }
        Braking();
        if (pitchModifier > 0.0f)
        {
            if (0.4f + pitchModifier/2.0f <= 1.4f)
                engineSound.pitch = 0.4f + pitchModifier / 2.0f;
            else
                engineSound.pitch = 1.4f;
        }
        carSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
    }

    void updateTorque()
    {
        float engineRPMRange = 0;
        if (engineData.maxTorqueRPM > currentRPM) {
            engineRPMRange = engineData.maxTorqueRPM - 1000;
            engineRPMRange = (currentRPM / engineRPMRange) * 100;
            engineTorque = engineData.minimumEngineTorque + ((engineRPMRange/100) * engineData.maximumEngineTorque);
        }
        if (engineData.maxTorqueRPM < currentRPM) {
            engineTorque = engineData.maximumEngineTorque;
        }
    }

    void Braking()
    {
        if (carData.braking)
        {
            wheelsDrive[0].brakeTorque = carData.brakeTorque;
            wheelsDrive[1].brakeTorque = carData.brakeTorque;
            steeringDrive[0].brakeTorque = carData.brakeTorque;
            steeringDrive[1].brakeTorque = carData.brakeTorque;
        } else
        {
            wheelsDrive[0].brakeTorque = 0;
            wheelsDrive[1].brakeTorque = 0;
            steeringDrive[0].brakeTorque = 0;
            steeringDrive[1].brakeTorque = 0;
        }
    }

    void Acceleration()
    {
        wheelTorque = (engineTorque * carData.throttle * carData.gearRatio[currentGear] * carData.gearRatio[6]);
        currentRPM = Mathf.Abs((wheelsDrive[0].rpm + wheelsDrive[1].rpm) / 2.0f * carData.gearRatio[currentGear] * carData.gearRatio[6]);
        if (currentRPM < 1000) {
            currentRPM = 1000;
        }
        if (currentRPM >= engineData.redline)
            wheelTorque = 0;
        pitchModifier = (currentRPM / maxRPM) * 2.0f;
        if (true /*&& carSpeed<maxCarSpeed*/)
        {
            if (currentGear == 0)
                wheelTorque = -1 * wheelTorque;
            wheelsDrive[0].motorTorque = wheelTorque * wheelsDrive[0].radius;
            wheelsDrive[1].motorTorque = wheelTorque * wheelsDrive[0].radius;

            wheelsDrive[0].brakeTorque = 0;
            wheelsDrive[1].brakeTorque = 0;
        }
    }
}
