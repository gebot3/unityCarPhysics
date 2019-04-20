using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public Transform targetTransform;
    private Rigidbody targetRigidbody;
    public GameObject target;
    public float distance = 3.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    public float smoothTime = 0.3f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;

    private void Awake() {
        updateTargetRigidbody();
    }

    void updateTargetRigidbody() {
        targetRigidbody = target.GetComponent<Rigidbody>();
    }

    void LateUpdate() {
        Vector3 velocity = targetRigidbody.velocity;
        Vector3 wantedPosition;
        if (followBehind)
            wantedPosition = targetTransform.TransformPoint(0, height, -distance);
        else
            wantedPosition = targetTransform.TransformPoint(0, height, distance);

            transform.position = Vector3.SmoothDamp(transform.position, wantedPosition ,ref velocity,smoothTime);

        if (smoothRotation) {
            Quaternion wantedRotation = Quaternion.LookRotation(targetTransform.position - transform.position, targetTransform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.smoothDeltaTime * rotationDamping);
        }
        else transform.LookAt(targetTransform, targetTransform.up);
    }
}
