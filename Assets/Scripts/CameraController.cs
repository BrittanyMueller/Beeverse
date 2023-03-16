using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // TODO create limits for x/z movement
    public float cameraSpeed = 300f;

    public float minFOV = 20;
    public float maxFOV = 80;
    private float _targetFOV;
    
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    
    private void Update() {
        MoveCamera();
        ZoomCamera();
    }

    private void MoveCamera() {
        // Reset move direction before next input
        var input = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) input.z = 1f;  // Move forward
        if (Input.GetKey(KeyCode.A)) input.x = -1f; // Move left
        if (Input.GetKey(KeyCode.S)) input.z = -1f; // Move back
        if (Input.GetKey(KeyCode.D)) input.x = 1f;  // Move right
        
        // Move direction relative to camera position
        var moveDirection = transform.forward * input.z + transform.right * input.x;
        transform.position += moveDirection * (cameraSpeed * Time.deltaTime);
    }

    private void ZoomCamera() {
        if (Input.mouseScrollDelta.y > 0) {
            // Positive scrolling forward (zoom in)
            _targetFOV += 5;
        }
        if (Input.mouseScrollDelta.y < 0) {
            // Negative indicates scrolling backward (zoom out)
            _targetFOV -= 5;
        }
        // Check that desired FOV does not exceed limits
        if (_targetFOV > maxFOV) _targetFOV = maxFOV;
        else if (_targetFOV < minFOV) _targetFOV = minFOV;
        virtualCamera.m_Lens.FieldOfView = _targetFOV;
    }
}
