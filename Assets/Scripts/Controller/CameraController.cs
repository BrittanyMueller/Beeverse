using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

  private float maxCameraSpeed = 325f;
  private float maxRotateSpeed = 75f;

  private float minFOV = 3;
  private float maxFOV = 40;

  private float minXRotation = 360 - 30;  // Note: this is because angles or stored [0,360]
  private float maxXRotation = 85;

  private float minXTravel = 50;
  private float maxXTravel = 950;
  private float minZTravel = 50;
  private float maxZTravel = 950;

  private float _targetFOV;

  [SerializeField]
  private CinemachineVirtualCamera virtualCamera;

  private void Start() {
    _targetFOV = 20;
  }
  private void Update() {
    RotateCamera();
    MoveCamera();
    ZoomCamera();
    RecentreCamera();
  }

  private void MoveCamera() {
    // Reset move direction before next input
    var input = new Vector3(0, 0, 0);

    if (Input.GetKey(KeyCode.W))
      input.z = 1f;  // Move forward
    if (Input.GetKey(KeyCode.A))
      input.x = -1f;  // Move left
    if (Input.GetKey(KeyCode.S))
      input.z = -1f;  // Move back
    if (Input.GetKey(KeyCode.D))
      input.x = 1f;  // Move right

    // Adjust camera speed based on FOV
    var cameraSpeed = maxCameraSpeed * _targetFOV / maxFOV;
    if (cameraSpeed < 120) {
      // Don't let camera speed fall below certain amount
      cameraSpeed += 15;
    }
    // Move direction relative to camera position
    var moveDirection = transform.forward * input.z + transform.right * input.x;
    moveDirection.y = 0;  // Lock y-axis when moving

    // Prevent the camera from moving off the terrain
    var deltaPos = moveDirection.normalized * (cameraSpeed * Time.deltaTime);
    var newPosition = transform.position + deltaPos;
    
    if (newPosition.x >= minXTravel && newPosition.x <= maxXTravel) {
      transform.position += new Vector3(deltaPos.x, 0,0);
    }
    if (newPosition.z >= minZTravel && newPosition.z <= maxZTravel) {
      transform.position +=  new Vector3(0, 0, deltaPos.z);
    }
  }

  private void RotateCamera() {
    var rotateDirection = 0f;
    if (Input.GetKey(KeyCode.Q))
      rotateDirection = 1f;
    if (Input.GetKey(KeyCode.E))
      rotateDirection = -1f;

    // Adjust rotate speed relative to how zoomed out
    var rotateSpeed = maxRotateSpeed * _targetFOV / maxFOV;
    switch (rotateSpeed) {
      // Further adjust for certain low thresholds
      case < 20:
        rotateSpeed += 5;
        break;
      case < 45:
        rotateSpeed += 10;
        break;
    }
    if (Input.GetMouseButton(1)) {
      var newAngle = transform.eulerAngles.x + rotateDirection * rotateSpeed * Time.deltaTime;
      if (!(newAngle < maxXRotation || newAngle > minXRotation)) {
        return;
      }
      transform.eulerAngles += new Vector3(rotateDirection * rotateSpeed * Time.deltaTime, 0, 0);
    } else {
      transform.eulerAngles += new Vector3(0, -rotateDirection * rotateSpeed * Time.deltaTime, 0);
    }
  }

  private void ZoomCamera() {

    var zoomAmount = 2;
    if (_targetFOV > 15) {
      // Adjust zoom amount to decrement faster when far away
      zoomAmount = 5;
    }

    // Disable zoom when scrolling UI elements
    if (EventSystem.current.IsPointerOverGameObject())
      return;

    if (Input.mouseScrollDelta.y > 0) {
      // Positive scrolling forward (zoom in)
      _targetFOV -= zoomAmount;
    }
    if (Input.mouseScrollDelta.y < 0) {
      // Negative indicates scrolling backward (zoom out)
      _targetFOV += zoomAmount;
    }
    // Check that desired FOV does not exceed limits
    if (_targetFOV > maxFOV)
      _targetFOV = maxFOV;
    else if (_targetFOV < minFOV)
      _targetFOV = minFOV;
    virtualCamera.m_Lens.FieldOfView = _targetFOV;
  }

  private void RecentreCamera() {
    if (Input.GetKey(KeyCode.Space)) {
      transform.eulerAngles = new Vector3(30, 0, 0);
      transform.position = new Vector3(500, 250, 80);;
    }
  }
}
