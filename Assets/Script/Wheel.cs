using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {


    WheelCollider wheelCollider;
    public AudioSource tireScreech;
    WheelFrictionCurve sidewayFriction,initSidewayFriction;

    float timer = 0.0f;

    void Awake()
    {
        tireScreech.volume = 0.0f;
        wheelCollider = GetComponent<WheelCollider>();
        initSidewayFriction = wheelCollider.sidewaysFriction;
        sidewayFriction = wheelCollider.sidewaysFriction;
        sidewayFriction.stiffness = 1.0f;
    }

    void FixedUpdate()
    {
        WheelHit wheelHit;
        timer += Time.fixedDeltaTime;

        if (wheelCollider.GetGroundHit(out wheelHit) && timer>0.2f)
        {
            timer = 0.0f;
            Debug.Log(gameObject +" "+ wheelHit.sidewaysSlip);
        }

        if (wheelCollider.GetGroundHit(out wheelHit))
        {
            if (Mathf.Abs(wheelHit.sidewaysSlip) > 0.3f)
            {
                if (tireScreech.volume < 0.25f)
                    tireScreech.volume += 0.001f;
                /*if (wheelCollider.sidewaysFriction.stiffness != initSidewayFriction.stiffness)
                    wheelCollider.sidewaysFriction = initSidewayFriction;*/
            }
            else
            {
                if (tireScreech.isPlaying && tireScreech.volume > 0.0f)
                {
                    tireScreech.volume -= 0.001f;
                }
                /*if (wheelCollider.sidewaysFriction.stiffness > 1.0f)
                    wheelCollider.sidewaysFriction = sidewayFriction;*/
            }
        }
    }

}