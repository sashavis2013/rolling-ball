using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAimer : MonoBehaviour {
    [SerializeField] float rotationSpeed = 45f;
    [SerializeField] Vector2 pitchClamp = new Vector2(-30f, 30f);
    [SerializeField] Vector2 yawClamp = new Vector2(-45f, 45f);
    Quaternion currentRotation;
    
    void Awake() => currentRotation = transform.rotation;
    
    public void UpdateAim(Vector2 inputDirection) {
        Vector3 rotationDelta = new Vector3(
            -inputDirection.y * rotationSpeed * Time.deltaTime, // Negative for natural drag
            inputDirection.x * rotationSpeed * Time.deltaTime,
            0
        );
    
        currentRotation *= Quaternion.Euler(rotationDelta);
    
        // Convert to local Euler angles for proper clamping
        Vector3 localEuler = currentRotation.eulerAngles;
        localEuler.x = ClampAngle(localEuler.x, pitchClamp.x, pitchClamp.y);
        localEuler.y = ClampAngle(localEuler.y, yawClamp.x, yawClamp.y);
        localEuler.z = 0;
    
        currentRotation = Quaternion.Euler(localEuler);
        transform.rotation = currentRotation;
    }
    
    float ClampAngle(float angle, float min, float max) {
        if (angle > 180) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}