using UnityEngine;
using System.Collections;

public class AerodynamicResistance : MonoBehaviour {

    public Vector3 coefficient;
    private Vector3 magnitude, absMagnitude, force;


	void FixedUpdate()
    {
        magnitude = transform.InverseTransformDirection(gameObject.GetComponent<Rigidbody>().velocity);
        absMagnitude = new Vector3(Mathf.Abs(magnitude.x), Mathf.Abs(magnitude.y), Mathf.Abs (magnitude.z));
        force = Vector3.Scale(Vector3.Scale(magnitude, absMagnitude), -2 * coefficient);
        gameObject.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(force));
    }
}
