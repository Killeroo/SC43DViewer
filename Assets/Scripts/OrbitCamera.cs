using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform Target;
    
    private Vector3 _cameraOffset;

    private void Start()
    {
        _cameraOffset = transform.position - Target.position;
        transform.LookAt(Target);
    }
    
    private void LateUpdate()
    {
        Quaternion cameraAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 5, Vector3.up);

        // Vector3 newPos = Target.position - (Target.forward * 5);
        
        Vector3 newPos = Target.position + _cameraOffset;
        _cameraOffset = cameraAngle * _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, 0.5f);
        transform.LookAt(Target);
    }
    
}
